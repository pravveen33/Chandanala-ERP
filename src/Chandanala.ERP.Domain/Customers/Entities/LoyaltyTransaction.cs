namespace Chandanala.ERP.Domain.Customers.Entities;

public enum LoyaltyTransactionType
{
    Earned = 1,
    Redeemed = 2,
    Adjusted = 3,
    Expired = 4
}

public class LoyaltyTransaction
{
    public Guid Id { get; private set; }

    public DateTime TransactionDate { get; private set; }

    public LoyaltyTransactionType TransactionType { get; private set; }

    public int Points { get; private set; }

    public string Description { get; private set; }

    public LoyaltyTransaction(
        LoyaltyTransactionType transactionType,
        int points,
        string description)
    {
        if (points <= 0)
            throw new ArgumentOutOfRangeException(nameof(points));

        Id = Guid.NewGuid();
        TransactionDate = DateTime.UtcNow;
        TransactionType = transactionType;
        Points = points;
        Description = description;
    }
}
