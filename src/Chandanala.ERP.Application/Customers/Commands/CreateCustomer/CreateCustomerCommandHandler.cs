using Chandanala.ERP.Application.Common.Interfaces;
using Chandanala.ERP.Application.Common.Interfaces.Persistence;
using Chandanala.ERP.Domain.Common.ValueObjects;
using Chandanala.ERP.Domain.Customers.Entities;
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
        var existingCustomer =
            await _customerRepository.GetByCustomerCodeAsync(
                request.CustomerCode,
                cancellationToken);

        if (existingCustomer is not null)
            throw new InvalidOperationException(
                $"Customer '{request.CustomerCode}' already exists.");

        var phone = new PhoneNumber(request.PhoneNumber);

        EmailAddress? email = null;

        if (!string.IsNullOrWhiteSpace(request.Email))
            email = new EmailAddress(request.Email);

        var address = new Address(
            request.AddressLine1,
            null,
            request.City,
            request.State,
            request.PostalCode);

        var customer = new Customer(
            request.CustomerCode,
            request.FullName,
            phone,
            email,
            address);

        await _customerRepository.AddAsync(
            customer,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return customer.Id;
    }
}
