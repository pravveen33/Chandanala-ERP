using MediatR;

namespace Chandanala.ERP.Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, Guid>
{
    public Task<Guid> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        // TODO:
        // 1. Validate request
        // 2. Create Customer aggregate
        // 3. Save using repository
        // 4. Publish domain events

        return Task.FromResult(Guid.NewGuid());
    }
}
