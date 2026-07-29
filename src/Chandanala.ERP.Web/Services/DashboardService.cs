using Chandanala.ERP.Web.Data;
using Chandanala.ERP.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Web.Services;

public sealed class DashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardSummary> GetDashboardAsync()
    {
        var today = DateTime.Today;
        var monthStart = new DateTime(today.Year, today.Month, 1);

        return new DashboardSummary
        {
            // Sales
            TodaySales = await _db.Sales
                .Where(s => s.SaleDate.Date == today)
                .SumAsync(s => (decimal?)s.NetAmount) ?? 0m,

            MonthlySales = await _db.Sales
                .Where(s => s.SaleDate >= monthStart)
                .SumAsync(s => (decimal?)s.NetAmount) ?? 0m,

            TotalBills = await _db.Sales.CountAsync(),

            AverageBillValue = await _db.Sales.AnyAsync()
                ? await _db.Sales.AverageAsync(s => s.NetAmount)
                : 0m,

            // Inventory
            TotalMedicines = await _db.Medicines.CountAsync(),

            SupplierCount = await _db.Suppliers.CountAsync(),

            ExpiredBatchCount = await _db.Batches
                .CountAsync(b => b.ExpiryDate < today),

            ExpiringSoonCount = await _db.Batches
                .CountAsync(b =>
                    b.ExpiryDate >= today &&
                    b.ExpiryDate <= today.AddDays(30)),

            // Placeholder until stock quantity logic is implemented
            LowStockCount = 0,

            // Placeholder until inventory valuation is implemented
            InventoryValue = 0m,

            // Placeholder until purchase dashboard is implemented
            MonthlyPurchaseValue = 0m,

            // Placeholder until customer module is implemented
            CustomerCount = 0,

            AuditEventsToday = await _db.AuditLogs
                .CountAsync(a => a.PerformedOn.Date == today),

            LastUpdated = DateTime.Now
        };
    }
}

