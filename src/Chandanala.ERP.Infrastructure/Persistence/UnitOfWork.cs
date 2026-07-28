using Chandanala.ERP.Application.Common.Interfaces;

namespace Chandanala.ERP.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CoreDbContext _context;

    public UnitOfWork(CoreDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
