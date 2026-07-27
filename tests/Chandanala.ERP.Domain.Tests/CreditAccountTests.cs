using Chandanala.ERP.Domain.Customers.Entities;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class CreditAccountTests
{
    [Fact]
    public void Constructor_Should_Set_CreditLimit()
    {
        var account = new CreditAccount(10000m);

        Assert.Equal(10000m, account.CreditLimit);
        Assert.Equal(0m, account.OutstandingBalance);
        Assert.Equal(10000m, account.AvailableCredit);
    }

    [Fact]
    public void Charge_Should_Increase_OutstandingBalance()
    {
        var account = new CreditAccount(10000m);

        account.Charge(2500m);

        Assert.Equal(2500m, account.OutstandingBalance);
        Assert.Equal(7500m, account.AvailableCredit);
    }

    [Fact]
    public void ReceivePayment_Should_Decrease_OutstandingBalance()
    {
        var account = new CreditAccount(10000m);

        account.Charge(4000m);
        account.ReceivePayment(1500m);

        Assert.Equal(2500m, account.OutstandingBalance);
        Assert.Equal(7500m, account.AvailableCredit);
    }

    [Fact]
    public void ReceivePayment_Should_Not_Go_Below_Zero()
    {
        var account = new CreditAccount(10000m);

        account.ReceivePayment(500m);

        Assert.Equal(0m, account.OutstandingBalance);
    }

    [Fact]
    public void Charge_Should_Throw_When_CreditLimit_Exceeded()
    {
        var account = new CreditAccount(1000m);

        Assert.Throws<InvalidOperationException>(
            () => account.Charge(1500m));
    }

    [Fact]
    public void UpdateCreditLimit_Should_Update_Limit()
    {
        var account = new CreditAccount(5000m);

        account.UpdateCreditLimit(8000m);

        Assert.Equal(8000m, account.CreditLimit);
    }

    [Fact]
    public void UpdateCreditLimit_Should_Throw_When_Below_OutstandingBalance()
    {
        var account = new CreditAccount(10000m);

        account.Charge(6000m);

        Assert.Throws<InvalidOperationException>(
            () => account.UpdateCreditLimit(5000m));
    }
}
