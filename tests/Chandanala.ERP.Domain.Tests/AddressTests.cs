using Chandanala.ERP.Domain.Common.ValueObjects;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class AddressTests
{
    [Fact]
    public void Valid_Address_Should_Be_Created()
    {
        var address = new Address(
            "12 MG Road",
            null,
            "Warangal",
            "Telangana",
            "506002");

        Assert.Equal("Warangal", address.City);
        Assert.Equal("India", address.Country);
    }

    [Fact]
    public void Missing_Line1_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new Address("", null, "Warangal", "Telangana", "506002"));
    }

    [Fact]
    public void Missing_City_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new Address("12 MG Road", null, "", "Telangana", "506002"));
    }

    [Fact]
    public void Missing_State_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new Address("12 MG Road", null, "Warangal", "", "506002"));
    }

    [Fact]
    public void Missing_PostalCode_Should_Throw()
    {
        Assert.Throws<ArgumentException>(() =>
            new Address("12 MG Road", null, "Warangal", "Telangana", ""));
    }
}
