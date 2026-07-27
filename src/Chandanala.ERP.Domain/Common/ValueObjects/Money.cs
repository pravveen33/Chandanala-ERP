namespace Chandanala.ERP.Domain.Common.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; init; }

    public string Currency { get; init; }

    public Money(decimal amount, string currency = "INR")
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }

    public static Money Zero(string currency = "INR")
        => new(0m, currency);

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException(
                "Money operations require matching currencies.");
    }

    public override string ToString()
        => $"{Currency} {Amount:N2}";
}
