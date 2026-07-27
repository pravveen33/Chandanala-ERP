namespace Chandanala.ERP.Domain.Customers.Entities;

public class LoyaltyAccount
{
    public Guid Id { get; private set; }

    public int PointsBalance { get; private set; }

    public bool IsActive { get; private set; }

    public LoyaltyAccount()
    {
        Id = Guid.NewGuid();
        PointsBalance = 0;
        IsActive = true;
    }

    public void AddPoints(int points)
    {
        if (points <= 0)
            throw new ArgumentOutOfRangeException(nameof(points));

        PointsBalance += points;
    }

    public void RedeemPoints(int points)
    {
        if (points <= 0)
            throw new ArgumentOutOfRangeException(nameof(points));

        if (points > PointsBalance)
            throw new InvalidOperationException("Insufficient loyalty points.");

        PointsBalance -= points;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
