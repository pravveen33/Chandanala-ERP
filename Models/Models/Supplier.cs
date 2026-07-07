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
}
