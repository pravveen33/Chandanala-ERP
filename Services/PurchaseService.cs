using Chandanala.ERP.Web.Data;
using Chandanala.ERP.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Web.Services;

public class PurchaseService
{
    private const string ModuleName = "Purchase";
    private const string ReferenceType = "Purchase";

    private readonly AppDbContext db;
    private readonly ILogger<PurchaseService> logger;

    public PurchaseService(AppDbContext db, ILogger<PurchaseService> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public async Task<List<PurchaseListItem>> GetAllAsync()
    {
        try
        {
            return await BuildListQuery()
                .OrderByDescending(p => p.PurchaseDate)
                .ThenByDescending(p => p.Id)
                .Take(100)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load purchases");
            throw new PurchaseServiceException("Could not load purchases. Please refresh the page.", ex);
        }
    }

    public async Task<PurchaseDetails?> GetByIdAsync(int id)
    {
        try
        {
            var purchase = await db.Purchases.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (purchase is null)
            {
                return null;
            }

            var items = await db.PurchaseItems.AsNoTracking()
                .Where(i => i.PurchaseId == id)
                .OrderBy(i => i.Id)
                .ToListAsync();

            return new PurchaseDetails(purchase, items);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load purchase {PurchaseId}", id);
            throw new PurchaseServiceException("Could not load this purchase. Please try again.", ex);
        }
    }

    public async Task<List<PurchaseListItem>> SearchAsync(string? searchText)
    {
        try
        {
            var query = BuildListQuery();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var term = searchText.Trim().ToLower();
                var hasDate = DateTime.TryParse(searchText.Trim(), out var parsedDate);
                var date = parsedDate.Date;

                query = query.Where(p =>
                    p.PurchaseNumber.ToLower().Contains(term) ||
                    p.InvoiceNumber.ToLower().Contains(term) ||
                    p.SupplierName.ToLower().Contains(term) ||
                    (hasDate && (p.PurchaseDate.Date == date || p.InvoiceDate.Date == date)));
            }

            return await query
                .OrderByDescending(p => p.PurchaseDate)
                .ThenByDescending(p => p.Id)
                .Take(100)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to search purchases with text {SearchText}", searchText);
            throw new PurchaseServiceException("Could not search purchases. Please try again.", ex);
        }
    }

    public async Task<Purchase> CreatePurchaseAsync(Purchase purchase, IReadOnlyCollection<PurchaseItem> items)
    {
        NormalizePurchase(purchase);
        NormalizeItems(items);
        await ValidatePurchaseAsync(purchase, items, null);

        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            purchase.Id = 0;
            purchase.PurchaseNumber = await GeneratePurchaseNumberAsync();
            purchase.Status = PurchaseStatus.Completed;
            RecalculateTotals(purchase, items);

            db.Purchases.Add(purchase);
            await db.SaveChangesAsync();

            foreach (var item in items)
            {
                item.Id = 0;
                item.PurchaseId = purchase.Id;
                CalculateLine(item);
                db.PurchaseItems.Add(item);
            }

            await db.SaveChangesAsync();

            foreach (var item in items)
            {
                await ApplyReceiptAsync(purchase, item);
            }

            AddAudit(purchase, AuditActions.Create, $"Created GRN {purchase.PurchaseNumber}");
            await db.SaveChangesAsync();
            await transaction.CommitAsync();

            return purchase;
        }
        catch (Exception ex) when (ex is not PurchaseValidationException)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Failed to create purchase {PurchaseNumber}", purchase.PurchaseNumber);
            throw new PurchaseServiceException("Could not save this purchase. Please check the details and try again.", ex);
        }
    }

    public async Task<Purchase> UpdatePurchaseAsync(Purchase purchase, IReadOnlyCollection<PurchaseItem> items)
    {
        NormalizePurchase(purchase);
        NormalizeItems(items);
        await ValidatePurchaseAsync(purchase, items, purchase.Id);

        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var existing = await db.Purchases.FirstOrDefaultAsync(p => p.Id == purchase.Id);
            if (existing is null || existing.Status == PurchaseStatus.Deleted)
            {
                throw new PurchaseValidationException("Purchase was not found or has already been deleted.");
            }

            if (existing.Status == PurchaseStatus.Cancelled)
            {
                throw new PurchaseValidationException("Cancelled purchases cannot be updated.");
            }

            var existingItems = await db.PurchaseItems
                .Where(i => i.PurchaseId == existing.Id)
                .ToListAsync();

            await ReverseReceiptAsync(existing, existingItems, "Purchase update reversal");

            db.PurchaseItems.RemoveRange(existingItems);

            existing.SupplierId = purchase.SupplierId;
            existing.InvoiceNumber = purchase.InvoiceNumber;
            existing.InvoiceDate = purchase.InvoiceDate;
            existing.PurchaseDate = purchase.PurchaseDate;
            existing.OtherCharges = purchase.OtherCharges;
            existing.RoundOff = purchase.RoundOff;
            existing.Remarks = purchase.Remarks;
            RecalculateTotals(existing, items);

            foreach (var item in items)
            {
                item.Id = 0;
                item.PurchaseId = existing.Id;
                CalculateLine(item);
                db.PurchaseItems.Add(item);
            }

            await db.SaveChangesAsync();

            foreach (var item in items)
            {
                await ApplyReceiptAsync(existing, item);
            }

            AddAudit(existing, AuditActions.Update, $"Updated GRN {existing.PurchaseNumber}");
            await db.SaveChangesAsync();
            await transaction.CommitAsync();

            return existing;
        }
        catch (Exception ex) when (ex is not PurchaseValidationException)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Failed to update purchase {PurchaseId}", purchase.Id);
            throw new PurchaseServiceException("Could not update this purchase. Please try again.", ex);
        }
    }

    public async Task CancelPurchaseAsync(int id, string? reason = null)
    {
        await ChangePurchaseStatusAsync(id, PurchaseStatus.Cancelled, AuditActions.Cancel, "Purchase Cancel", reason);
    }

    public async Task DeletePurchaseAsync(int id)
    {
        await ChangePurchaseStatusAsync(id, PurchaseStatus.Deleted, AuditActions.Delete, "Purchase Delete", null);
    }

    private IQueryable<PurchaseListItem> BuildListQuery()
    {
        return from purchase in db.Purchases.AsNoTracking()
               join supplier in db.Suppliers.AsNoTracking()
                   on purchase.SupplierId equals supplier.Id
               where purchase.Status != PurchaseStatus.Deleted
               select new PurchaseListItem
               {
                   Id = purchase.Id,
                   PurchaseNumber = purchase.PurchaseNumber,
                   SupplierName = supplier.SupplierName,
                   InvoiceNumber = purchase.InvoiceNumber,
                   InvoiceDate = purchase.InvoiceDate,
                   PurchaseDate = purchase.PurchaseDate,
                   GrandTotal = purchase.GrandTotal,
                   Status = purchase.Status
               };
    }

    private async Task ChangePurchaseStatusAsync(
        int id,
        string status,
        string auditAction,
        string auditModuleAction,
        string? reason)
    {
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var purchase = await db.Purchases.FirstOrDefaultAsync(p => p.Id == id);
            if (purchase is null || purchase.Status == PurchaseStatus.Deleted)
            {
                throw new PurchaseValidationException("Purchase was not found or has already been deleted.");
            }

            if (purchase.Status == PurchaseStatus.Cancelled)
            {
                throw new PurchaseValidationException("Purchase is already cancelled.");
            }

            var items = await db.PurchaseItems
                .Where(i => i.PurchaseId == id)
                .ToListAsync();

            await ReverseReceiptAsync(purchase, items, auditModuleAction);

            purchase.Status = status;
            purchase.Remarks = AppendRemark(purchase.Remarks, reason);

            AddAudit(purchase, auditAction, $"{auditModuleAction}: {purchase.PurchaseNumber}");
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex) when (ex is not PurchaseValidationException)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Failed to change purchase {PurchaseId} status to {Status}", id, status);
            throw new PurchaseServiceException("Could not update this purchase. Please try again.", ex);
        }
    }

    private async Task ApplyReceiptAsync(Purchase purchase, PurchaseItem item)
    {
        var receivedQuantity = item.Quantity + item.FreeQuantity;

        var batch = await db.Batches.FirstOrDefaultAsync(b =>
            !b.IsDeleted &&
            b.MedicineId == item.MedicineId &&
            b.BatchNo.ToLower() == item.BatchNumber.ToLower());

        if (batch is null)
        {
            batch = new Batch
            {
                MedicineId = item.MedicineId,
                BatchNo = item.BatchNumber,
                ExpiryDate = ToUtcDate(item.ExpiryDate),
                ManufacturingDate = item.ManufactureDate.HasValue ? ToUtcDate(item.ManufactureDate.Value) : null,
                PurchaseItemId = item.Id,
                PurchaseRate = item.PurchaseRate,
                MRP = item.MRP,
                QuantityReceived = receivedQuantity,
                QuantityAvailable = receivedQuantity,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            db.Batches.Add(batch);
            await db.SaveChangesAsync();
        }
        else
        {
            if (batch.ExpiryDate.Date != item.ExpiryDate.Date)
            {
                throw new PurchaseValidationException($"Batch {item.BatchNumber} already exists with a different expiry date.");
            }

            batch.QuantityReceived += receivedQuantity;
            batch.QuantityAvailable += receivedQuantity;
            batch.PurchaseRate = item.PurchaseRate;
            batch.MRP = item.MRP;
            batch.PurchaseItemId = item.Id;
            batch.ModifiedDate = DateTime.UtcNow;
        }

        db.StockTransactions.Add(new StockTransaction
        {
            MedicineId = item.MedicineId,
            BatchId = batch.Id,
            TransactionType = StockTransactionTypes.Purchase,
            Quantity = receivedQuantity,
            ReferenceType = ReferenceType,
            ReferenceNumber = purchase.PurchaseNumber,
            ReferenceId = item.Id,
            TransactionDate = ToUtcDate(purchase.PurchaseDate),
            Remarks = $"GRN {purchase.PurchaseNumber}"
        });
    }

    private async Task ReverseReceiptAsync(Purchase purchase, IReadOnlyCollection<PurchaseItem> items, string reason)
    {
        foreach (var item in items)
        {
            var receivedQuantity = item.Quantity + item.FreeQuantity;
            var batch = await db.Batches.FirstOrDefaultAsync(b =>
                !b.IsDeleted &&
                b.MedicineId == item.MedicineId &&
                b.BatchNo.ToLower() == item.BatchNumber.ToLower());

            if (batch is null)
            {
                throw new PurchaseValidationException($"Batch {item.BatchNumber} could not be found for reversal.");
            }

            if (batch.QuantityAvailable < receivedQuantity)
            {
                throw new PurchaseValidationException($"Cannot reverse batch {item.BatchNumber} because some stock has already been sold or adjusted.");
            }

            batch.QuantityAvailable -= receivedQuantity;
            batch.QuantityReceived -= receivedQuantity;
            batch.ModifiedDate = DateTime.UtcNow;

            db.StockTransactions.Add(new StockTransaction
            {
                MedicineId = item.MedicineId,
                BatchId = batch.Id,
                TransactionType = StockTransactionTypes.Adjustment,
                Quantity = -receivedQuantity,
                ReferenceType = ReferenceType,
                ReferenceNumber = purchase.PurchaseNumber,
                ReferenceId = item.Id,
                TransactionDate = DateTime.UtcNow,
                Remarks = reason
            });
        }
    }

    private async Task ValidatePurchaseAsync(Purchase purchase, IReadOnlyCollection<PurchaseItem> items, int? existingId)
    {
        if (purchase.SupplierId <= 0)
        {
            throw new PurchaseValidationException("Supplier is required.");
        }

        if (!await db.Suppliers.AsNoTracking().AnyAsync(s => s.Id == purchase.SupplierId && !s.IsDeleted && s.IsActive))
        {
            throw new PurchaseValidationException("Selected supplier is not active.");
        }

        if (string.IsNullOrWhiteSpace(purchase.InvoiceNumber))
        {
            throw new PurchaseValidationException("Invoice Number is required.");
        }

        if (purchase.InvoiceDate == default)
        {
            throw new PurchaseValidationException("Invoice Date is required.");
        }

        if (await db.Purchases.AsNoTracking().AnyAsync(p =>
            p.SupplierId == purchase.SupplierId &&
            p.InvoiceNumber.ToLower() == purchase.InvoiceNumber.ToLower() &&
            p.Status != PurchaseStatus.Deleted &&
            (!existingId.HasValue || p.Id != existingId.Value)))
        {
            throw new PurchaseValidationException("This supplier already has a purchase with the same Invoice Number.");
        }

        if (!string.IsNullOrWhiteSpace(purchase.PurchaseNumber) &&
            await db.Purchases.AsNoTracking().AnyAsync(p =>
                p.PurchaseNumber.ToLower() == purchase.PurchaseNumber.ToLower() &&
                (!existingId.HasValue || p.Id != existingId.Value)))
        {
            throw new PurchaseValidationException("Duplicate Purchase Number is not allowed.");
        }

        if (!items.Any())
        {
            throw new PurchaseValidationException("Add at least one purchase item.");
        }

        foreach (var item in items)
        {
            await ValidateItemAsync(item);
        }
    }

    private async Task ValidateItemAsync(PurchaseItem item)
    {
        if (item.MedicineId <= 0)
        {
            throw new PurchaseValidationException("Medicine is required for every item.");
        }

        if (!await db.Medicines.AsNoTracking().AnyAsync(m => m.Id == item.MedicineId && !m.IsDeleted && m.IsActive))
        {
            throw new PurchaseValidationException("One or more selected medicines are inactive.");
        }

        if (string.IsNullOrWhiteSpace(item.BatchNumber))
        {
            throw new PurchaseValidationException("Batch Number is required for every item.");
        }

        if (item.ExpiryDate == default)
        {
            throw new PurchaseValidationException("Expiry Date is required for every item.");
        }

        if (item.ExpiryDate.Date <= DateTime.Today)
        {
            throw new PurchaseValidationException($"Batch {item.BatchNumber} is expired or expires today.");
        }

        if (item.Quantity <= 0)
        {
            throw new PurchaseValidationException("Quantity must be greater than zero.");
        }

        if (item.FreeQuantity < 0)
        {
            throw new PurchaseValidationException("Free Quantity cannot be negative.");
        }

        if (item.PurchaseRate <= 0)
        {
            throw new PurchaseValidationException("Purchase Rate must be greater than zero.");
        }

        if (item.MRP < item.PurchaseRate)
        {
            throw new PurchaseValidationException("MRP cannot be less than Purchase Rate.");
        }

        if (item.SellingPrice > item.MRP)
        {
            throw new PurchaseValidationException("Selling Price cannot exceed MRP.");
        }

        if (item.GSTPercentage < 0 || item.GSTPercentage > 100)
        {
            throw new PurchaseValidationException("GST must be between 0 and 100.");
        }

        if (item.DiscountPercentage < 0 || item.DiscountPercentage > 100)
        {
            throw new PurchaseValidationException("Discount must be between 0 and 100.");
        }
    }

    private async Task<string> GeneratePurchaseNumberAsync()
    {
        var today = DateTime.UtcNow.Date;
        var prefix = $"GRN-{today:yyyyMMdd}-";
        var count = await db.Purchases.AsNoTracking()
            .CountAsync(p => p.PurchaseNumber.StartsWith(prefix));

        var next = count + 1;
        string purchaseNumber;

        do
        {
            purchaseNumber = $"{prefix}{next:0000}";
            next++;
        }
        while (await db.Purchases.AsNoTracking().AnyAsync(p => p.PurchaseNumber == purchaseNumber));

        return purchaseNumber;
    }

    private static void RecalculateTotals(Purchase purchase, IReadOnlyCollection<PurchaseItem> items)
    {
        foreach (var item in items)
        {
            CalculateLine(item);
        }

        purchase.Subtotal = items.Sum(i => i.Quantity * i.PurchaseRate);
        purchase.DiscountAmount = items.Sum(i => (i.Quantity * i.PurchaseRate) * (i.DiscountPercentage / 100m));
        purchase.GSTAmount = items.Sum(i =>
        {
            var taxable = (i.Quantity * i.PurchaseRate) * (1 - i.DiscountPercentage / 100m);
            return taxable * (i.GSTPercentage / 100m);
        });
        purchase.GrandTotal = purchase.Subtotal - purchase.DiscountAmount + purchase.GSTAmount + purchase.OtherCharges + purchase.RoundOff;
    }

    private static void CalculateLine(PurchaseItem item)
    {
        var gross = item.Quantity * item.PurchaseRate;
        var discount = gross * (item.DiscountPercentage / 100m);
        var taxable = gross - discount;
        var gst = taxable * (item.GSTPercentage / 100m);
        item.LineTotal = taxable + gst;
    }

    private static void NormalizePurchase(Purchase purchase)
    {
        purchase.PurchaseNumber = NormalizeOptional(purchase.PurchaseNumber) ?? string.Empty;
        purchase.InvoiceNumber = NormalizeRequired(purchase.InvoiceNumber).ToUpperInvariant();
        purchase.Remarks = NormalizeOptional(purchase.Remarks) ?? string.Empty;
        purchase.InvoiceDate = ToUtcDate(purchase.InvoiceDate);
        purchase.PurchaseDate = ToUtcDate(purchase.PurchaseDate == default ? DateTime.Today : purchase.PurchaseDate);
    }

    private static void NormalizeItems(IEnumerable<PurchaseItem> items)
    {
        foreach (var item in items)
        {
            item.BatchNumber = NormalizeRequired(item.BatchNumber).ToUpperInvariant();
            item.ExpiryDate = ToUtcDate(item.ExpiryDate);
            item.ManufactureDate = item.ManufactureDate.HasValue ? ToUtcDate(item.ManufactureDate.Value) : null;
        }
    }

    private static string NormalizeRequired(string? value) => value?.Trim() ?? string.Empty;

    private static string? NormalizeOptional(string? value)
    {
        var trimmed = value?.Trim();
        return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
    }

    private static DateTime ToUtcDate(DateTime value)
    {
        return DateTime.SpecifyKind(value.Date, DateTimeKind.Utc);
    }

    private static string AppendRemark(string existingRemarks, string? note)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            return existingRemarks;
        }

        return string.IsNullOrWhiteSpace(existingRemarks)
            ? note.Trim()
            : $"{existingRemarks.Trim()} | {note.Trim()}";
    }

    private void AddAudit(Purchase purchase, string action, string details)
    {
        db.AuditLogs.Add(new AuditLog
        {
            Module = ModuleName,
            EntityName = nameof(Purchase),
            EntityId = purchase.Id,
            Action = action,
            Details = details
        });
    }
}

public class PurchaseListItem
{
    public int Id { get; set; }

    public string PurchaseNumber { get; set; } = string.Empty;

    public string SupplierName { get; set; } = string.Empty;

    public string InvoiceNumber { get; set; } = string.Empty;

    public DateTime InvoiceDate { get; set; }

    public DateTime PurchaseDate { get; set; }

    public decimal GrandTotal { get; set; }

    public string Status { get; set; } = string.Empty;

    public string PurchaseDateText { get; set; } = string.Empty;

    public string InvoiceDateText { get; set; } = string.Empty;
}

public record PurchaseDetails(Purchase Purchase, List<PurchaseItem> Items);

public class PurchaseValidationException : Exception
{
    public PurchaseValidationException(string message) : base(message)
    {
    }
}

public class PurchaseServiceException : Exception
{
    public PurchaseServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
