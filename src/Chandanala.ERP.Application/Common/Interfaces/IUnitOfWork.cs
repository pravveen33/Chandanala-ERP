namespace Chandanala.ERP.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
