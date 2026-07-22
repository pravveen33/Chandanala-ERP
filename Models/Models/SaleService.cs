using Chandanala.ERP.Web.Data;
using Microsoft.EntityFrameworkCore;
using Chandanala.ERP.Web.Models;
namespace Chandanala.ERP.Web.Services;

public class SaleService
{
    private readonly AppDbContext _context;

    public SaleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddSaleAsync(Sale sale)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Sale>> GetSalesAsync()
    {
        return await _context.Sales
            .OrderByDescending(x => x.SaleDate)
            .ToListAsync();
    }

    public async Task<Sale?> GetSaleAsync(int id)
    {
        return await _context.Sales
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}