using System.ComponentModel.DataAnnotations;

namespace Chandanala.ERP.Web.Models;

/// <summary>
/// A complete point-in-time snapshot of a Supplier's master data, written just
/// before an update is applied. This is the audit trail for the Distributor
/// Master itself ("what did this distributor's details look like on date X"),
/// separate from Purchase-level snapshots — a Purchase copies distributor
/// values directly onto itself at save time rather than referencing this
/// table, so historical purchases stay self-contained even if this table is
/// archived, pruned, or retained under a different policy.
/// </summary>
public class SupplierHistory
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    /// <summary>
    /// The Supplier.CurrentVersion at the moment this snapshot was taken —
    /// i.e. this row is "version N", and the update that triggered this
    /// snapshot produced version N+1 on the live Supplier record.
    /// </summary>
    public int VersionNumber { get; set; }

    [StringLength(30)]
    public string SupplierCode { get; set; } = string.Empty;

    [StringLength(200)]
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

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    // Placeholder until user/auth context is wired in — replace with the
    // logged-in user's identity once authentication is added (matches the
    // same placeholder pattern used by AuditLog.PerformedBy).
    [StringLength(100)]
    public string ChangedBy { get; set; } = "System";

    [StringLength(500)]
    public string? Reason { get; set; }

    public Supplier? Supplier { get; set; }
}
