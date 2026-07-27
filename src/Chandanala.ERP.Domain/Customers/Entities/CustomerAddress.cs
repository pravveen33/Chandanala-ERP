using Chandanala.ERP.Domain.Common.ValueObjects;

namespace Chandanala.ERP.Domain.Customers.Entities;

public class CustomerAddress
{
    public Guid Id { get; private set; }

    public Address Address { get; private set; }

    public bool IsPrimary { get; private set; }

    public string AddressType { get; private set; }

    public CustomerAddress(
        Address address,
        string addressType = "Home",
        bool isPrimary = true)
    {
        Id = Guid.NewGuid();
        Address = address;
        AddressType = addressType;
        IsPrimary = isPrimary;
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }

    public void SetPrimary(bool isPrimary)
    {
        IsPrimary = isPrimary;
    }
}
