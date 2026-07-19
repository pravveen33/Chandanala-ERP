namespace Chandanala.Core.Identity.Domain.Interfaces;

public interface IUserRepository
{
    Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task AddAsync(UserEntity user, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}