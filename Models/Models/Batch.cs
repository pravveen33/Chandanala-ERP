using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chandanala.ERP.Web.Models
{
    /// <summary>
    /// A single received batch/lot of a Medicine. This is what makes FEFO (First-Expiry-
    /// First-Out) picking possible at sale time — a Medicine on its own has no concept of
    /// "which physical units expire when"; a Batch does.
    ///
    /// QuantityAvailable is a maintained running-total cache for fast reads (list screens,
    /// stock checks at sale time) — it is kept in sync with StockTransaction whenever a
    /// transaction against this batch is recorded. StockTransaction is the authoritative
    /// ledger; if the two ever disagree, QuantityAvailable can be rebuilt by summing this
    /// batch's StockTransaction.Quantity values. Never mutate QuantityAvailable directly
    /// without also writing the corresponding StockTransaction row.
    /// </summary>
    public class Batch
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Medicine is required")]
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "Batch Number is required")]
        [StringLength(50)]
        public string BatchNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expiry Date is required")]
        public DateTime ExpiryDate { get; set; }

        public DateTime? ManufacturingDate { get; set; }

        // Nullable: a batch can originate from a real Purchase (once that module is fully
        // wired) or from a manual opening-stock / adjustment entry, which has no purchase.
        public int? PurchaseItemId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Purchase Rate cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchaseRate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "MRP cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MRP { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityReceived { get; set; }

        // Maintained cache — see class remarks above.
        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Convenience flag for UI highlighting — not persisted, computed on read.
        /// A batch is "near expiry" inside a configurable window (default 90 days),
        /// which is when FEFO logic and staff attention should prioritize clearing it.
        /// </summary>
        [NotMapped]
        public bool IsNearExpiry => (ExpiryDate.Date - DateTime.Today).TotalDays <= 90 && !IsExpired;

        [NotMapped]
        public bool IsExpired => ExpiryDate.Date < DateTime.Today;
    }
}
