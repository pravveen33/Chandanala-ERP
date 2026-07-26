using Chandanala.ERP.Web.Shared.MasterData.Entities;

namespace Chandanala.ERP.Web.Shared.MasterData.Repositories;

public interface IMasterRepository<T>
    where T : MasterEntity
{
    Task<List<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    Task<T?> GetByCodeAsync(string code);

    Task AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(Guid id);

    Task<bool> ExistsAsync(string code);
}
