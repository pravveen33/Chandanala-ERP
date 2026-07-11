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

    public async Task<Supplier> UpdateAsync(Supplier supplier, string? reason = null)
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

            var changedRegulatedFields = GetChangedRegulatedFields(existing, supplier);
            if (changedRegulatedFields.Count > 0)
            {
                var changeRequest = new SupplierChangeRequest
                {
                    SupplierId = existing.Id,
                    SupplierCode = supplier.SupplierCode,
                    SupplierName = supplier.SupplierName,
                    GSTIN = supplier.GSTIN,
                    DrugLicenseNumber = supplier.DrugLicenseNumber,
                    ContactPerson = supplier.ContactPerson,
                    MobileNumber = supplier.MobileNumber,
                    Email = supplier.Email,
                    Address = supplier.Address,
                    City = supplier.City,
                    State = supplier.State,
                    Pincode = supplier.Pincode,
                    CreditDays = supplier.CreditDays,
                    CreditLimit = supplier.CreditLimit,
                    IsActive = supplier.IsActive,
                    ChangedRegulatedFields = string.Join(", ", changedRegulatedFields),
                    ExpectedRowVersion = supplier.RowVersion,
                    RequestedBy = "System",
                    RequestReason = reason
                };

                db.SupplierChangeRequests.Add(changeRequest);
                await db.SaveChangesAsync();

                throw new SupplierApprovalRequiredException(changeRequest.Id, changeRequest.ChangedRegulatedFields);
            }

            // Enforce optimistic concurrency using the token the caller loaded
            // this record with — not the token on the freshly-fetched `existing`.
            // If someone else saved a change in between, this mismatch is what
            // makes SaveChangesAsync throw DbUpdateConcurrencyException below.
            db.Entry(existing).Property(e => e.RowVersion).OriginalValue = supplier.RowVersion;

            // Snapshot the current (about-to-be-overwritten) state into history
            // before applying the new values, tagged with the version it was at.
            db.SupplierHistories.Add(new SupplierHistory
            {
                SupplierId = existing.Id,
                VersionNumber = existing.CurrentVersion,
                SupplierCode = existing.SupplierCode,
                SupplierName = existing.SupplierName,
                GSTIN = existing.GSTIN,
                DrugLicenseNumber = existing.DrugLicenseNumber,
                ContactPerson = existing.ContactPerson,
                MobileNumber = existing.MobileNumber,
                Email = existing.Email,
                Address = existing.Address,
                City = existing.City,
                State = existing.State,
                Pincode = existing.Pincode,
                CreditDays = existing.CreditDays,
                CreditLimit = existing.CreditLimit,
                IsActive = existing.IsActive,
                ChangedBy = "System",
                Reason = reason
            });

            existing.CurrentVersion += 1;
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
            existing.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            await AddAuditAsync(existing, AuditActions.Update);

            return existing;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogWarning(ex, "Concurrency conflict while updating supplier {SupplierId}", supplier.Id);
            throw new SupplierServiceException("This supplier was changed by someone else after you loaded it. Please refresh and try again.", ex);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error while updating supplier {SupplierId}", supplier.Id);
            throw new SupplierServiceException("Could not update this supplier. Please check for duplicate values and try again.", ex);
        }
        catch (Exception ex) when (ex is not SupplierValidationException and not SupplierApprovalRequiredException)
        {
            logger.LogError(ex, "Failed to update supplier {SupplierId}", supplier.Id);
            throw new SupplierServiceException("Something went wrong while updating this supplier. Please try again.", ex);
        }
    }

    /// <summary>
    /// Full version history for a distributor, most recent first — the point-in-time
    /// audit trail described in the Distributor History design (separate from
    /// Purchase-level commercial snapshots, which copy values directly onto the
    /// Purchase itself rather than referencing this table).
    /// </summary>
    public async Task<List<SupplierHistory>> GetHistoryAsync(int supplierId)
    {
        try
        {
            return await db.SupplierHistories
                .AsNoTracking()
                .Where(h => h.SupplierId == supplierId)
                .OrderByDescending(h => h.VersionNumber)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load supplier history for {SupplierId}", supplierId);
            throw new SupplierServiceException("Could not load this distributor's change history. Please try again.", ex);
        }
    }

    /// <summary>
    /// The Master Data Review Queue for distributors — every request still
    /// awaiting a decision, oldest first so reviewers clear the backlog in order.
    /// </summary>
    public async Task<List<SupplierChangeRequest>> GetPendingChangeRequestsAsync()
    {
        try
        {
            return await db.SupplierChangeRequests
                .AsNoTracking()
                .Include(r => r.Supplier)
                .Where(r => r.Status == SupplierChangeRequestStatus.Pending)
                .OrderBy(r => r.RequestedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load pending supplier change requests");
            throw new SupplierServiceException("Could not load the pending approvals list. Please try again.", ex);
        }
    }

    /// <summary>
    /// Applies a pending request's proposed values to the live Supplier record,
    /// writing a SupplierHistory snapshot the same way a direct UpdateAsync
    /// would — approval is just a delayed, reviewed version of the same update.
    /// </summary>
    public async Task<Supplier> ApproveChangeRequestAsync(int requestId, string? reviewNotes = null)
    {
        try
        {
            var request = await db.SupplierChangeRequests
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request is null)
            {
                throw new SupplierValidationException("Change request was not found.");
            }

            if (request.Status != SupplierChangeRequestStatus.Pending)
            {
                throw new SupplierValidationException($"This request has already been {request.Status.ToLowerInvariant()} and can't be approved again.");
            }

            var existing = await db.Suppliers
                .FirstOrDefaultAsync(s => s.Id == request.SupplierId && !s.IsDeleted);

            if (existing is null)
            {
                throw new SupplierValidationException("The distributor this request was raised for was not found or has since been deleted.");
            }

            // Same concurrency guard as a direct update, but checked against
            // the token captured when the request was submitted rather than
            // one just loaded — if something else changed the supplier in the
            // meantime, this throws instead of silently overwriting it.
            db.Entry(existing).Property(e => e.RowVersion).OriginalValue = request.ExpectedRowVersion;

            db.SupplierHistories.Add(new SupplierHistory
            {
                SupplierId = existing.Id,
                VersionNumber = existing.CurrentVersion,
                SupplierCode = existing.SupplierCode,
                SupplierName = existing.SupplierName,
                GSTIN = existing.GSTIN,
                DrugLicenseNumber = existing.DrugLicenseNumber,
                ContactPerson = existing.ContactPerson,
                MobileNumber = existing.MobileNumber,
                Email = existing.Email,
                Address = existing.Address,
                City = existing.City,
                State = existing.State,
                Pincode = existing.Pincode,
                CreditDays = existing.CreditDays,
                CreditLimit = existing.CreditLimit,
                IsActive = existing.IsActive,
                ChangedBy = "System",
                Reason = $"Approved change request #{request.Id} ({request.ChangedRegulatedFields})"
            });

            existing.CurrentVersion += 1;
            existing.SupplierCode = request.SupplierCode;
            existing.SupplierName = request.SupplierName;
            existing.GSTIN = request.GSTIN;
            existing.DrugLicenseNumber = request.DrugLicenseNumber;
            existing.ContactPerson = request.ContactPerson;
            existing.MobileNumber = request.MobileNumber;
            existing.Email = request.Email;
            existing.Address = request.Address;
            existing.City = request.City;
            existing.State = request.State;
            existing.Pincode = request.Pincode;
            existing.CreditDays = request.CreditDays;
            existing.CreditLimit = request.CreditLimit;
            existing.IsActive = request.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            request.Status = SupplierChangeRequestStatus.Approved;
            request.ReviewedBy = "System";
            request.ReviewedAt = DateTime.UtcNow;
            request.ReviewNotes = reviewNotes;

            await db.SaveChangesAsync();
            await AddAuditAsync(existing, AuditActions.Update);

            return existing;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogWarning(ex, "Concurrency conflict while approving change request {RequestId}", requestId);
            throw new SupplierServiceException("This distributor was changed by someone else since the request was submitted. Please review the current record and resubmit.", ex);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error while approving change request {RequestId}", requestId);
            throw new SupplierServiceException("Could not approve this request. Please check for duplicate values and try again.", ex);
        }
        catch (Exception ex) when (ex is not SupplierValidationException)
        {
            logger.LogError(ex, "Failed to approve change request {RequestId}", requestId);
            throw new SupplierServiceException("Something went wrong while approving this request. Please try again.", ex);
        }
    }

    /// <summary>Rejects a pending request — per the governance design, this makes NO change to the Supplier record at all.</summary>
    public async Task RejectChangeRequestAsync(int requestId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new SupplierValidationException("A reason is required to reject a change request.");
        }

        try
        {
            var request = await db.SupplierChangeRequests
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request is null)
            {
                throw new SupplierValidationException("Change request was not found.");
            }

            if (request.Status != SupplierChangeRequestStatus.Pending)
            {
                throw new SupplierValidationException($"This request has already been {request.Status.ToLowerInvariant()} and can't be rejected.");
            }

            request.Status = SupplierChangeRequestStatus.Rejected;
            request.ReviewedBy = "System";
            request.ReviewedAt = DateTime.UtcNow;
            request.ReviewNotes = reason.Trim();

            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error while rejecting change request {RequestId}", requestId);
            throw new SupplierServiceException("Could not reject this request. Please try again.", ex);
        }
        catch (Exception ex) when (ex is not SupplierValidationException)
        {
            logger.LogError(ex, "Failed to reject change request {RequestId}", requestId);
            throw new SupplierServiceException("Something went wrong while rejecting this request. Please try again.", ex);
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

    /// <summary>
    /// Regulated fields per the Distributor governance design: changes to
    /// these require approval before taking effect, unlike the rest of the
    /// Supplier record which updates immediately. Compares against `proposed`
    /// after Normalize() has already run on it, so both sides are consistently
    /// trimmed/cased (GSTIN is stored upper-invariant).
    /// </summary>
    private static List<string> GetChangedRegulatedFields(Supplier existing, Supplier proposed)
    {
        var changed = new List<string>();

        if (!string.Equals(existing.GSTIN, proposed.GSTIN, StringComparison.Ordinal))
        {
            changed.Add("GSTIN");
        }

        if (!string.Equals(existing.DrugLicenseNumber, proposed.DrugLicenseNumber, StringComparison.Ordinal))
        {
            changed.Add("Drug License Number");
        }

        return changed;
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

/// <summary>
/// Not an error — signals that an update was diverted into a pending
/// SupplierChangeRequest because it touched a regulated field, and needs
/// approval before it takes effect. Callers (e.g. Suppliers.razor) should
/// catch this distinctly from real failures and show an informational
/// "submitted for approval" message rather than an error.
/// </summary>
public class SupplierApprovalRequiredException : Exception
{
    public int RequestId { get; }
    public string ChangedFields { get; }

    public SupplierApprovalRequiredException(int requestId, string changedFields)
        : base($"This change touches regulated field(s) ({changedFields}) and requires approval before it takes effect.")
    {
        RequestId = requestId;
        ChangedFields = changedFields;
    }
}
