using Chandanala.ERP.Domain.Common.ValueObjects;
using Chandanala.ERP.Domain.Customers.Entities;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class CustomerTests
{
    [Fact]
    public void Constructor_Should_Create_Active_Customer()
    {
        var customer = new Customer(
            "CUST001",
            "John Doe",
            new PhoneNumber("9876543210"));

        Assert.Equal("CUST001", customer.CustomerCode);
        Assert.Equal("John Doe", customer.FullName);
        Assert.True(customer.IsActive);
    }

    [Fact]
    public void Deactivate_Should_Set_IsActive_To_False()
    {
        var customer = new Customer(
            "CUST001",
            "John Doe",
            new PhoneNumber("9876543210"));

        customer.Deactivate();

        Assert.False(customer.IsActive);
    }

    [Fact]
    public void Activate_Should_Set_IsActive_To_True()
    {
        var customer = new Customer(
            "CUST001",
            "John Doe",
            new PhoneNumber("9876543210"));

        customer.Deactivate();
        customer.Activate();

        Assert.True(customer.IsActive);
    }

    [Fact]
    public void Update_Email_Should_Replace_Email()
    {
        var customer = new Customer(
            "CUST001",
            "John Doe",
            new PhoneNumber("9876543210"));

        customer.UpdateEmail(new EmailAddress("john@example.com"));

        Assert.Equal("john@example.com", customer.Email!.Value);
    }

    [Fact]
    public void Update_Address_Should_Replace_Address()
    {
        var customer = new Customer(
            "CUST001",
            "John Doe",
            new PhoneNumber("9876543210"));

        var address = new Address(
            "12 MG Road",
            null,
            "Warangal",
            "Telangana",
            "506002");

        customer.UpdateAddress(address);

        Assert.Equal("Warangal", customer.Address!.City);
    }
}
