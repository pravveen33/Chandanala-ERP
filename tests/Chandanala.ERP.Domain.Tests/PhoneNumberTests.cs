using Chandanala.ERP.Domain.Common.ValueObjects;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class PhoneNumberTests
{
    [Fact]
    public void Valid_Number_Should_Be_Accepted()
    {
        var phone = new PhoneNumber("9876543210");

        Assert.Equal("9876543210", phone.Value);
    }

    [Fact]
    public void Empty_Number_Should_Throw()
    {
        Assert.Throws<ArgumentException>(
            () => new PhoneNumber(""));
    }

    [Fact]
    public void Invalid_Number_Should_Throw()
    {
        Assert.Throws<ArgumentException>(
            () => new PhoneNumber("12345"));
    }

    [Fact]
    public void Number_With_Spaces_Should_Be_Trimmed()
    {
        var phone = new PhoneNumber(" 9876543210 ");

        Assert.Equal("9876543210", phone.Value);
    }
}
