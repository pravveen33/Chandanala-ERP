using System.ComponentModel.DataAnnotations;

namespace Chandanala.ERP.Web.Models
{
    /// <summary>
    /// Append-only stock movement ledger. This is the source of truth for "how much of
    /// this batch do we actually have" — Batch.QuantityAvailable is a cache derived from
    /// summing these rows per BatchId. Rows are never updated or deleted after creation;
    /// corrections are made by inserting an offsetting Adjustment transaction, the same
    /// way an accounting ledger handles corrections — this preserves a full audit trail
    /// of every stock movement, which matters for a pharmacy given regulatory expectations
    /// around traceability.
    /// </summary>
    public class StockTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MedicineId { get; set; }

        [Required]
        public int BatchId { get; set; }

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; } = string.Empty; // see StockTransactionTypes

        /// <summary>
        /// Signed quantity: positive = stock in (purchase, return-in, opening, positive
        /// adjustment), negative = stock out (sale, return-out, damage/expiry write-off,
        /// negative adjustment). A single signed column, rather than separate In/Out
        /// columns, makes "current balance = SUM(Quantity)" a trivial, always-correct
        /// aggregate with no risk of the two columns drifting out of sync with each other.
        /// </summary>
        public int Quantity { get; set; }

        [StringLength(30)]
        public string? ReferenceType { get; set; } // e.g. "Purchase", "Sale", "Manual" — free-form until those modules exist

        [StringLength(30)]
        public string? ReferenceNumber { get; set; }

        public int? ReferenceId { get; set; } // e.g. PurchaseItem.Id or a future SaleItem.Id

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Remarks { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Canonical values for StockTransaction.TransactionType, kept as constants for the
    /// same reason AuditActions exists — prevents casing/spelling drift across call sites.
    /// </summary>
    public static class StockTransactionTypes
    {
        public const string OpeningStock = "Opening";
        public const string Purchase = "Purchase";
        public const string Sale = "Sale";
        public const string ReturnIn = "ReturnIn";       // customer returned goods to us
        public const string ReturnOut = "ReturnOut";     // we returned goods to a supplier
        public const string Adjustment = "Adjustment";   // manual correction, positive or negative
        public const string Writeoff = "Writeoff";       // expired/damaged stock removed
        public const string SaleCancellation = "SaleCancellation"; // stock restored from a voided sale
    }
}
