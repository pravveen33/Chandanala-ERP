using Chandanala.ERP.Domain.Common.ValueObjects;

namespace Chandanala.ERP.Domain.Customers.Entities;

public class Customer
{
    // Required by EF Core
private Customer()
{
    CustomerCode = string.Empty;
    FullName = string.Empty;

    PhoneNumber = null!;
    Email = null;
    Address = null;

    CreatedOn = default;
    IsActive = true;
}
    public Guid Id { get; private set; }

    public string CustomerCode { get; private set; }

    public string FullName { get; private set; }

    public PhoneNumber PhoneNumber { get; private set; }

    public EmailAddress? Email { get; private set; }

    public Address? Address { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public bool IsActive { get; private set; }

    public Customer(
        string customerCode,
        string fullName,
        PhoneNumber phoneNumber,
        EmailAddress? email = null,
        Address? address = null)
    {
        Id = Guid.NewGuid();
        CustomerCode = customerCode;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        CreatedOn = DateTime.UtcNow;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }

    public void UpdateEmail(EmailAddress email)
    {
        Email = email;
    }
}


