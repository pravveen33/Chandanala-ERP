using System.ComponentModel.DataAnnotations;

namespace Chandanala.ERP.Web.Models;

/// <summary>
/// A pending edit to a Supplier that touches a regulated field (GSTIN, Drug
/// License Number) and therefore can't apply immediately. Holds the FULL
/// proposed field set (not just the changed ones) so approval has a complete
/// target state to apply in one step — see SupplierService.ApproveChangeRequestAsync.
///
/// Business rule (per the Distributor governance design):
///     User edits a regulated field
///             │
///             ▼
///        Pending Review  (this record, Status = Pending)
///             │
///             ├── Approved → Supplier updated + SupplierHistory snapshot written
///             └── Rejected → No change to Supplier at all
/// </summary>
public class SupplierChangeRequest
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    [Required, StringLength(30)]
    public string SupplierCode { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string SupplierName { get; set; } = string.Empty;

    [StringLength(15)]
    public string? GSTIN { get; set; }

    [StringLength(100)]
    public string? DrugLicenseNumber { get; set; }

    [StringLength(200)]
    public string? ContactPerson { get; set; }

    [StringLength(20)]
    public string? MobileNumber { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(10)]
    public string? Pincode { get; set; }

    public int CreditDays { get; set; }

    public decimal CreditLimit { get; set; }

    public bool IsActive { get; set; }

    /// <summary>Human-readable list of which regulated fields changed, e.g. "GSTIN, Drug License Number" — for display in the review queue.</summary>
    [StringLength(200)]
    public string ChangedRegulatedFields { get; set; } = string.Empty;

    [StringLength(20)]
    public string Status { get; set; } = SupplierChangeRequestStatus.Pending;

    /// <summary>
    /// The Supplier.RowVersion (xmin) this request was built against. Used the
    /// same way UpdateAsync uses the caller's RowVersion: if the Supplier was
    /// changed by something else between request and approval, approving
    /// throws DbUpdateConcurrencyException instead of silently overwriting it.
    /// </summary>
    public uint ExpectedRowVersion { get; set; }

    // Placeholder until user/auth context is wired in — see SupplierService.
    [Required, StringLength(100)]
    public string RequestedBy { get; set; } = "System";

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    [StringLength(500)]
    public string? RequestReason { get; set; }

    [StringLength(100)]
    public string? ReviewedBy { get; set; }

    public DateTime? ReviewedAt { get; set; }

    /// <summary>Free-text notes from the reviewer — the rejection reason when Status = Rejected, or an optional approval comment when Status = Approved.</summary>
    [StringLength(500)]
    public string? ReviewNotes { get; set; }

    public Supplier? Supplier { get; set; }
}

public static class SupplierChangeRequestStatus
{
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
}
