using MediatR;

namespace Chandanala.ERP.Application.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string CustomerCode,
    string FullName,
    string PhoneNumber,
    string? Email,
    string AddressLine1,
    string City,
    string State,
    string PostalCode
) : IRequest<Guid>;
