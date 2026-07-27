using Chandanala.ERP.Web.Models;

namespace Chandanala.ERP.Web.Modules.Procurement.SupplierMaster.Services;

public interface ISupplierMasterService
{
    Task<List<Supplier>> GetAllAsync();

    Task<Supplier?> GetByIdAsync(int id);

    Task CreateAsync(Supplier supplier);

    Task UpdateAsync(Supplier supplier);

    Task DeleteAsync(int id);
}