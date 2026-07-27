using Chandanala.ERP.Web.Shared.MasterData.Entities;

namespace Chandanala.ERP.Web.Shared.MasterData.Services;

public interface IMasterService<T>
    where T : MasterEntity
{
    Task<List<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    Task CreateAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(Guid id);
}
