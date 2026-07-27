using Chandanala.ERP.Domain.Common.ValueObjects;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class EmailAddressTests
{
    [Fact]
    public void Valid_Email_Should_Be_Accepted()
    {
        var email = new EmailAddress("admin@example.com");

        Assert.Equal("admin@example.com", email.Value);
    }

    [Fact]
    public void Invalid_Email_Should_Throw()
    {
        Assert.Throws<ArgumentException>(
            () => new EmailAddress("invalid-email"));
    }

    [Fact]
    public void Empty_Email_Should_Throw()
    {
        Assert.Throws<ArgumentException>(
            () => new EmailAddress(""));
    }

    [Fact]
    public void Email_Should_Be_Normalized_To_Lowercase()
    {
        var email = new EmailAddress("ADMIN@Example.COM");

        Assert.Equal("admin@example.com", email.Value);
    }
}
