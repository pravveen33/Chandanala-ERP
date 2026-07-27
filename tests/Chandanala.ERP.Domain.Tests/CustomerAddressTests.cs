using Chandanala.ERP.Domain.Common.ValueObjects;
using Chandanala.ERP.Domain.Customers.Entities;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class CustomerAddressTests
{
    [Fact]
    public void Constructor_Should_Create_Primary_Address()
    {
        var address = new Address(
            "12 MG Road",
            null,
            "Warangal",
            "Telangana",
            "506002");

        var customerAddress = new CustomerAddress(address);

        Assert.True(customerAddress.IsPrimary);
        Assert.Equal("Home", customerAddress.AddressType);
    }

    [Fact]
    public void SetPrimary_Should_Update_Status()
    {
        var address = new Address(
            "12 MG Road",
            null,
            "Warangal",
            "Telangana",
            "506002");

        var customerAddress = new CustomerAddress(address);

        customerAddress.SetPrimary(false);

        Assert.False(customerAddress.IsPrimary);
    }

    [Fact]
    public void UpdateAddress_Should_Replace_Address()
    {
        var address1 = new Address(
            "12 MG Road",
            null,
            "Warangal",
            "Telangana",
            "506002");

        var address2 = new Address(
            "45 Park Street",
            null,
            "Hyderabad",
            "Telangana",
            "500001");

        var customerAddress = new CustomerAddress(address1);

        customerAddress.UpdateAddress(address2);

        Assert.Equal("Hyderabad", customerAddress.Address.City);
    }
}
