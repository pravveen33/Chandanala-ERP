using Chandanala.ERP.Application.Common.Interfaces.Persistence;
using Chandanala.ERP.Domain.Customers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly CoreDbContext _context;

    public CustomerRepository(CoreDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        await _context.Set<Customer>()
            .AddAsync(customer, cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Customer>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetByCustomerCodeAsync(
        string customerCode,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Customer>()
            .FirstOrDefaultAsync(
                x => x.CustomerCode == customerCode,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<Customer>()
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        _context.Set<Customer>().Update(customer);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        _context.Set<Customer>().Remove(customer);
        return Task.CompletedTask;
    }
}
