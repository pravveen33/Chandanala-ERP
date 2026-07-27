using Chandanala.ERP.Web.Data;
using Chandanala.ERP.Web.Shared.MasterData.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Web.Shared.MasterData.Repositories;

public class MasterRepository<T> : IMasterRepository<T>
    where T : MasterEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public MasterRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet
            .Where(x => x.IsActive)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<bool> ExistsAsync(string code)
    {
        return await _dbSet.AnyAsync(x => x.Code == code);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);

        if (entity is null)
            return;

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}
