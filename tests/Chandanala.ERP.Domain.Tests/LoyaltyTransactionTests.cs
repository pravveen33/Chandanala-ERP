using Chandanala.ERP.Domain.Customers.Entities;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class LoyaltyTransactionTests
{
    [Fact]
    public void Constructor_Should_Create_Earned_Transaction()
    {
        var transaction = new LoyaltyTransaction(
            LoyaltyTransactionType.Earned,
            100,
            "Purchase Reward");

        Assert.Equal(LoyaltyTransactionType.Earned, transaction.TransactionType);
        Assert.Equal(100, transaction.Points);
        Assert.Equal("Purchase Reward", transaction.Description);
    }

    [Fact]
    public void Constructor_Should_Create_Redeemed_Transaction()
    {
        var transaction = new LoyaltyTransaction(
            LoyaltyTransactionType.Redeemed,
            50,
            "Discount Redemption");

        Assert.Equal(LoyaltyTransactionType.Redeemed, transaction.TransactionType);
        Assert.Equal(50, transaction.Points);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Points_Are_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new LoyaltyTransaction(
                LoyaltyTransactionType.Earned,
                0,
                "Invalid"));
    }
}
