using System.ComponentModel.DataAnnotations;
using Chandanala.ERP.Web.Data;
using Chandanala.ERP.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Web.Services;

public class SupplierService
{
    private const string ModuleName = "Supplier Master";

    private readonly AppDbContext db;
    private readonly ILogger<SupplierService> logger;

    public SupplierService(AppDbContext db, ILogger<SupplierService> logger)
    {
        this.db = db;
        this.logger = logger;
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        try
        {
            return await db.Suppliers
                .AsNoTracking()
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.SupplierName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load suppliers");
            throw new SupplierServiceException("Could not load suppliers. Please refresh the page.", ex);
        }
    }

    public async Task<List<Supplier>> SearchAsync(string? searchText)
    {
        try
        {
            var query = db.Suppliers
                .AsNoTracking()
                .Where(s => !s.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var term = searchText.Trim().ToLower();
                query = query.Where(s =>
                    s.SupplierCode.ToLower().Contains(term) ||
                    s.SupplierName.ToLower().Contains(term) ||
                    (s.ContactPerson != null && s.ContactPerson.ToLower().Contains(term)) ||
                    (s.MobileNumber != null && s.MobileNumber.ToLower().Contains(term)) ||
                    (s.GSTIN != null && s.GSTIN.ToLower().Contains(term)) ||
                    (s.City != null && s.City.ToLower().Contains(term)) ||
                    (s.State != null && s.State.ToLower().Contains(term)));
            }

            return await query
                .OrderBy(s => s.SupplierName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to search suppliers with text {SearchText}", searchText);
            throw new SupplierServiceException("Could not search suppliers. Please try again.", ex);
        }
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        try
        {
            return await db.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load supplier {SupplierId}", id);
            throw new SupplierServiceException("Could not load this supplier. Please try again.", ex);
        }
    }

    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        Normalize(supplier);
        await ValidateSupplierAsync(supplier);

        try
        {
            supplier.Id = 0;
            supplier.IsDeleted = false;

            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();

            await AddAuditAsync(supplier, AuditActions.Create);
            return supplier;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error while creating supplier {SupplierCode}", supplier.SupplierCode);
            throw new SupplierServiceException("Could not save this supplier. Please check for duplicate values and try again.", ex);
        }
        catch (Exception ex) when (ex is not SupplierValidationException)
        {
            logger.LogError(ex, "Failed to create supplier {SupplierCode}", supplier.SupplierCode);
            throw new SupplierServiceException("Something went wrong while saving this supplier. Please try again.", ex);
        }
    }

    public async Task<Supplier> UpdateAsync(Supplier supplier)
    {
        Normalize(supplier);
        await ValidateSupplierAsync(supplier);

        try
        {
            var existing = await db.Suppliers
                .FirstOrDefaultAsync(s => s.Id == supplier.Id && !s.IsDeleted);

            if (existing is null)
            {
                throw new SupplierValidationException("Supplier was not found or has already been deleted.");
            }

            existing.SupplierCode = supplier.SupplierCode;
            existing.SupplierName = supplier.SupplierName;
            existing.GSTIN = supplier.GSTIN;
            existing.DrugLicenseNumber = supplier.DrugLicenseNumber;
            existing.ContactPerson = supplier.ContactPerson;
            existing.MobileNumber = supplier.MobileNumber;
            existing.Email = supplier.Email;
            existing.Address = supplier.Address;
            existing.City = supplier.City;
            existing.State = supplier.State;
            existing.Pincode = supplier.Pincode;
            existing.CreditDays = supplier.CreditDays;
            existing.CreditLimit = supplier.CreditLimit;
            existing.IsActive = supplier.IsActive;

            await db.SaveChangesAsync();
            await AddAuditAsync(existing, AuditActions.Update);

            return existing;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error while updating supplier {SupplierId}", supplier.Id);
            throw new SupplierServiceException("Could not update this supplier. Please check for duplicate values and try again.", ex);
        }
        catch (Exception ex) when (ex is not SupplierValidationException)
        {
            logger.LogError(ex, "Failed to update supplier {SupplierId}", supplier.Id);
            throw new SupplierServiceException("Something went wrong while updating this supplier. Please try again.", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var supplier = await db.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

            if (supplier is null)
            {
                throw new SupplierValidationException("Supplier was not found or has already been deleted.");
            }

            var hasPurchases = await db.Purchases
                .AsNoTracking()
                .AnyAsync(p => p.SupplierId == id);

            if (hasPurchases)
            {
                throw new SupplierValidationException("Cannot delete this supplier because purchases are recorded against it.");
            }

            supplier.IsDeleted = true;
            supplier.IsActive = false;

            await db.SaveChangesAsync();
            await AddAuditAsync(supplier, AuditActions.Delete);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error while deleting supplier {SupplierId}", id);
            throw new SupplierServiceException("Could not delete this supplier. Please try again.", ex);
        }
        catch (Exception ex) when (ex is not SupplierValidationException)
        {
            logger.LogError(ex, "Failed to delete supplier {SupplierId}", id);
            throw new SupplierServiceException("Something went wrong while deleting this supplier. Please try again.", ex);
        }
    }

    public async Task<bool> ExistsAsync(string supplierCode, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(supplierCode))
        {
            return false;
        }

        var normalizedCode = supplierCode.Trim().ToLower();

        return await db.Suppliers
            .AsNoTracking()
            .AnyAsync(s => !s.IsDeleted
                && s.SupplierCode.ToLower() == normalizedCode
                && (!excludeId.HasValue || s.Id != excludeId.Value));
    }

    private async Task ValidateSupplierAsync(Supplier supplier)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(supplier);

        if (!Validator.TryValidateObject(supplier, validationContext, validationResults, true))
        {
            throw new SupplierValidationException(string.Join(" ", validationResults.Select(r => r.ErrorMessage)));
        }

        if (await ExistsAsync(supplier.SupplierCode, supplier.Id == 0 ? null : supplier.Id))
        {
            throw new SupplierValidationException("A supplier with this Supplier Code already exists.");
        }
    }

    private async Task AddAuditAsync(Supplier supplier, string action)
    {
        db.AuditLogs.Add(new AuditLog
        {
            Module = ModuleName,
            EntityName = nameof(Supplier),
            EntityId = supplier.Id,
            Action = action,
            Details = $"{supplier.SupplierCode} - {supplier.SupplierName}"
        });

        await db.SaveChangesAsync();
    }

    private static void Normalize(Supplier supplier)
    {
        supplier.SupplierCode = NormalizeRequired(supplier.SupplierCode).ToUpperInvariant();
        supplier.SupplierName = NormalizeRequired(supplier.SupplierName);
        supplier.GSTIN = NormalizeOptional(supplier.GSTIN)?.ToUpperInvariant();
        supplier.DrugLicenseNumber = NormalizeOptional(supplier.DrugLicenseNumber);
        supplier.ContactPerson = NormalizeOptional(supplier.ContactPerson);
        supplier.MobileNumber = NormalizeOptional(supplier.MobileNumber);
        supplier.Email = NormalizeOptional(supplier.Email);
        supplier.Address = NormalizeOptional(supplier.Address);
        supplier.City = NormalizeOptional(supplier.City);
        supplier.State = NormalizeOptional(supplier.State);
        supplier.Pincode = NormalizeOptional(supplier.Pincode);
    }

    private static string NormalizeRequired(string? value) => value?.Trim() ?? string.Empty;

    private static string? NormalizeOptional(string? value)
    {
        var trimmed = value?.Trim();
        return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
    }
}

public class SupplierValidationException : Exception
{
    public SupplierValidationException(string message) : base(message)
    {
    }
}

public class SupplierServiceException : Exception
{
    public SupplierServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
