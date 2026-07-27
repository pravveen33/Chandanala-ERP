using Chandanala.ERP.Domain.Common.ValueObjects;

namespace Chandanala.ERP.Domain.Customers.Entities;

public class CreditAccount
{
    public Guid Id { get; private set; }

    public decimal CreditLimit { get; private set; }

    public decimal OutstandingBalance { get; private set; }

    public decimal AvailableCredit => CreditLimit - OutstandingBalance;

    public CreditAccount(decimal creditLimit)
    {
        if (creditLimit < 0)
            throw new ArgumentOutOfRangeException(nameof(creditLimit));

        Id = Guid.NewGuid();
        CreditLimit = creditLimit;
        OutstandingBalance = 0;
    }

    public void Charge(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        if (OutstandingBalance + amount > CreditLimit)
            throw new InvalidOperationException("Credit limit exceeded.");

        OutstandingBalance += amount;
    }

    public void ReceivePayment(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        OutstandingBalance -= amount;

        if (OutstandingBalance < 0)
            OutstandingBalance = 0;
    }

    public void UpdateCreditLimit(decimal newLimit)
    {
        if (newLimit < OutstandingBalance)
            throw new InvalidOperationException(
                "Credit limit cannot be below outstanding balance.");

        CreditLimit = newLimit;
    }
}
