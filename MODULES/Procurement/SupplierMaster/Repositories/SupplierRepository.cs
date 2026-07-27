using Chandanala.ERP.Web.Data;
using Chandanala.ERP.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Web.Modules.Procurement.SupplierMaster.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _db;

    public SupplierRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        return await _db.Suppliers
            .Where(s => !s.IsDeleted)
            .OrderBy(s => s.SupplierName)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _db.Suppliers
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
    }

    public async Task<Supplier?> GetByCodeAsync(string supplierCode)
    {
        return await _db.Suppliers
            .FirstOrDefaultAsync(s =>
                s.SupplierCode == supplierCode &&
                !s.IsDeleted);
    }

    public async Task<bool> ExistsAsync(string supplierCode)
    {
        return await _db.Suppliers
            .AnyAsync(s =>
                s.SupplierCode == supplierCode &&
                !s.IsDeleted);
    }

    public async Task AddAsync(Supplier supplier)
    {
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        _db.Suppliers.Update(supplier);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var supplier = await GetByIdAsync(id);

        if (supplier is null)
            return;

        supplier.IsDeleted = true;
        supplier.IsActive = false;

        await _db.SaveChangesAsync();
    }
}