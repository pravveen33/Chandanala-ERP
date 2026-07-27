using Chandanala.ERP.Web.Models;
using Chandanala.ERP.Web.Modules.Procurement.SupplierMaster.Repositories;

namespace Chandanala.ERP.Web.Modules.Procurement.SupplierMaster.Services;

public class SupplierMasterService : ISupplierMasterService
{
    private readonly ISupplierRepository _repository;

    public SupplierMasterService(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task CreateAsync(Supplier supplier)
    {
        if (await _repository.ExistsAsync(supplier.SupplierCode))
            throw new InvalidOperationException("Supplier code already exists.");

        supplier.CreatedAt = DateTime.UtcNow;
        supplier.IsActive = true;
        supplier.IsDeleted = false;

        await _repository.AddAsync(supplier);
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        supplier.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(supplier);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}