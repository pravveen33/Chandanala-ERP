using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chandanala.ERP.Web.Models
{
    /// <summary>
    /// A single line on a Sale invoice. Rate and GstPercent are SNAPSHOTS taken at the
    /// moment of sale, deliberately not live-linked to Medicine.SellingPrice/GstPercent —
    /// an invoice must reflect the price and tax rate that applied when it was issued,
    /// even if the medicine's price changes later. Recomputing historical invoices from
    /// current Medicine data would silently corrupt financial records.
    /// </summary>
    public class SaleItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SaleId { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int BatchId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        // Snapshot — pre-tax unit price at time of sale.
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        // Snapshot — GST% at time of sale.
        [Column(TypeName = "decimal(5,2)")]
        public decimal GstPercent { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        // Taxable value = (Quantity * Rate) - DiscountAmount
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GstAmount { get; set; }

        // NetAmount = Amount + GstAmount
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }
    }
}
