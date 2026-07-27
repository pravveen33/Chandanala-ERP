using Chandanala.ERP.Application.Common.Interfaces;
using Chandanala.ERP.Application.Common.Interfaces.Persistence;
using MediatR;

namespace Chandanala.ERP.Application.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler
    : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        // Domain object creation will be added once the aggregate
        // constructor/factory is finalised.

        // Example:
        // var customer = Customer.Create(...);

        // await _customerRepository.AddAsync(customer, cancellationToken);
        // await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Guid.NewGuid();
    }
}
