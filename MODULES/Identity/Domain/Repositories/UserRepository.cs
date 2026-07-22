using Chandanala.Core.Identity.Domain.Interfaces;
using Chandanala.ERP.Web.Models;

namespace Chandanala.Core.Identity.Domain.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(
        User user,
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync(
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}