using Chandanala.ERP.Web.Models;

namespace Chandanala.ERP.Web.Modules.Procurement.SupplierMaster.Repositories;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllAsync();

    Task<Supplier?> GetByIdAsync(int id);

    Task<Supplier?> GetByCodeAsync(string supplierCode);

    Task AddAsync(Supplier supplier);

    Task UpdateAsync(Supplier supplier);

    Task DeleteAsync(int id);

    Task<bool> ExistsAsync(string supplierCode);
}