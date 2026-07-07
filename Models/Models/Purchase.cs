namespace Chandanala.ERP.Web.Models;

public class Purchase
{
    public int Id { get; set; }

    public string PurchaseNumber { get; set; } = string.Empty;

    public int SupplierId { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;
    
    public DateTime InvoiceDate { get; set; } = DateTime.Today;

    public DateTime PurchaseDate { get; set; } = DateTime.Today;

    public decimal Subtotal { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal GSTAmount { get; set; }

    public decimal OtherCharges { get; set; }

    public decimal RoundOff { get; set; }

    public decimal GrandTotal { get; set; }

    public string Status { get; set; } = PurchaseStatus.Completed;

    public string Remarks { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}

public static class PurchaseStatus
{
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";
    public const string Deleted = "Deleted";
}
