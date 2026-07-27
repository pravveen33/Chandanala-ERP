using Chandanala.ERP.Domain.Customers.Entities;

namespace Chandanala.ERP.Application.Common.Interfaces.Persistence;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Customer?> GetByCustomerCodeAsync(
        string customerCode,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Customer>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Customer customer,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Customer customer,
        CancellationToken cancellationToken = default);
}
