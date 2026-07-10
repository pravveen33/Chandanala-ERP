using System.ComponentModel.DataAnnotations;

namespace Chandanala.ERP.Web.Models;

public class Supplier
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string SupplierCode { get; set; } = string.Empty;

    [Required]
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

    [EmailAddress]
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

    [Range(0, 3650)]
    public int CreditDays { get; set; }

    [Range(0, 999999999)]
    public decimal CreditLimit { get; set; }

    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Business-level version counter. Incremented on every update; the value
    /// just before the increment is what gets stamped onto the SupplierHistory
    /// snapshot created for that update, so history rows read as "version N was
    /// replaced at ChangedAt".
    /// </summary>
    public int CurrentVersion { get; set; } = 1;

    /// <summary>
    /// Optimistic concurrency token, mapped directly onto PostgreSQL's system
    /// column `xmin`. No dedicated column/migration needed for this — Postgres
    /// already maintains xmin on every row. EF Core will include the original
    /// value in the UPDATE's WHERE clause and throw DbUpdateConcurrencyException
    /// if the row changed since it was read, as long as callers round-trip this
    /// value (load it, keep it on the edited object, pass it back on save).
    /// Fully configured via Fluent API in AppDbContext.OnModelCreating — no
    /// attribute needed here, since [Timestamp] carries SQL-Server-rowversion
    /// assumptions that don't apply to xmin.
    /// </summary>
    public uint RowVersion { get; set; }
}
