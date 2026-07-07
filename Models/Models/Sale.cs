using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chandanala.ERP.Web.Models
{
    /// <summary>
    /// Sale invoice header. Deliberately has NO IsDeleted/soft-delete field, unlike other
    /// models in this app — an invoice must remain permanently traceable by its
    /// InvoiceNumber for tax/audit purposes even if voided. Use Status = Cancelled instead
    /// of deleting; cancellation reverses the associated stock movement rather than
    /// erasing the record of the sale having existed.
    ///
    /// CustomerId is a nullable placeholder for a future Customer/Distributor master —
    /// it is not yet backed by a table. Until that exists, CustomerName/CustomerMobile
    /// carry the walk-in customer's details inline, which is how most pharmacy POS
    /// transactions work day-to-day anyway.
    /// </summary>
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        // Placeholder FK for a future Customer master — not enforced at the DB level yet.
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Customer Name is required")]
        [StringLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(20)]
        public string? CustomerMobile { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }

        [StringLength(20)]
        public string PaymentMode { get; set; } = "Cash";

        // Free-text reference (e.g. prescription number/date) — not a file upload or
        // validation gate. Medicine.PrescriptionRequired is not currently enforced
        // against this field; that enforcement is a deliberate follow-up decision,
        // not an oversight, since it affects checkout UX and needs its own design pass.
        [StringLength(100)]
        public string? PrescriptionReference { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = SaleStatus.Completed;

        [StringLength(500)]
        public string? Remarks { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public DateTime? CancelledDate { get; set; }

        [StringLength(500)]
        public string? CancellationReason { get; set; }
    }

    public static class SaleStatus
    {
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
    }
}
