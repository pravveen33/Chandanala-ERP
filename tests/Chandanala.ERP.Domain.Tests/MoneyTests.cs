using Chandanala.ERP.Domain.Common.ValueObjects;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class MoneyTests
{
    [Fact]
    public void Constructor_Should_Set_Amount_And_Currency()
    {
        var money = new Money(100m);

        Assert.Equal(100m, money.Amount);
        Assert.Equal("INR", money.Currency);
    }

    [Fact]
    public void Add_Should_Return_Sum()
    {
        var first = new Money(100m);
        var second = new Money(50m);

        var result = first.Add(second);

        Assert.Equal(150m, result.Amount);
        Assert.Equal("INR", result.Currency);
    }

    [Fact]
    public void Subtract_Should_Return_Difference()
    {
        var first = new Money(100m);
        var second = new Money(40m);

        var result = first.Subtract(second);

        Assert.Equal(60m, result.Amount);
    }

    [Fact]
    public void Zero_Should_Return_Zero_Amount()
    {
        var money = Money.Zero();

        Assert.Equal(0m, money.Amount);
        Assert.Equal("INR", money.Currency);
    }

    [Fact]
    public void Add_Should_Throw_When_Currencies_Differ()
    {
        var inr = new Money(100m, "INR");
        var usd = new Money(50m, "USD");

        Assert.Throws<InvalidOperationException>(() => inr.Add(usd));
    }
}
