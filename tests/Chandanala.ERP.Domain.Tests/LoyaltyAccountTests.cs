using Chandanala.ERP.Domain.Customers.Entities;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class LoyaltyAccountTests
{
    [Fact]
    public void Constructor_Should_Create_Active_Account()
    {
        var account = new LoyaltyAccount();

        Assert.True(account.IsActive);
        Assert.Equal(0, account.PointsBalance);
    }

    [Fact]
    public void AddPoints_Should_Increase_Balance()
    {
        var account = new LoyaltyAccount();

        account.AddPoints(100);

        Assert.Equal(100, account.PointsBalance);
    }

    [Fact]
    public void RedeemPoints_Should_Decrease_Balance()
    {
        var account = new LoyaltyAccount();

        account.AddPoints(100);
        account.RedeemPoints(40);

        Assert.Equal(60, account.PointsBalance);
    }

    [Fact]
    public void RedeemPoints_Should_Throw_When_Insufficient()
    {
        var account = new LoyaltyAccount();

        Assert.Throws<InvalidOperationException>(
            () => account.RedeemPoints(10));
    }

    [Fact]
    public void Activate_Deactivate_Should_Update_Status()
    {
        var account = new LoyaltyAccount();

        account.Deactivate();
        Assert.False(account.IsActive);

        account.Activate();
        Assert.True(account.IsActive);
    }
}
