namespace Chandanala.ERP.Web.Models;

public class PurchaseItem
{
    public int Id { get; set; }

    public int PurchaseId { get; set; }

    public int MedicineId { get; set; }

    public string BatchNumber { get; set; } = string.Empty;

    public DateTime? ManufactureDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public decimal PurchaseRate { get; set; }

    public decimal MRP { get; set; }

    public decimal SellingPrice { get; set; }

    public decimal GSTPercentage { get; set; }

    public int Quantity { get; set; }

    public int FreeQuantity { get; set; }

    public decimal DiscountPercentage { get; set; }

    public decimal LineTotal { get; set; }
}
