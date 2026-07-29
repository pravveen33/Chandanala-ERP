namespace Chandanala.ERP.Web.Models;

public class DashboardSummary
{
    // Sales
    public decimal TodaySales { get; set; }
    public decimal MonthlySales { get; set; }
    public int TotalBills { get; set; }
    public decimal AverageBillValue { get; set; }

    // Inventory
    public decimal InventoryValue { get; set; }
    public int TotalMedicines { get; set; }
    public int LowStockCount { get; set; }
    public int ExpiredBatchCount { get; set; }
    public int ExpiringSoonCount { get; set; }

    // Purchases
    public decimal MonthlyPurchaseValue { get; set; }
    public int SupplierCount { get; set; }

    // Customers
    public int CustomerCount { get; set; }

    // Compliance
    public int AuditEventsToday { get; set; }

    // Last Updated
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}
