namespace Chandanala.ERP.Domain.Customers.Entities;

public class CustomerLedger
{
    public Guid Id { get; private set; }

    public DateTime TransactionDate { get; private set; }

    public string Description { get; private set; }

    public decimal DebitAmount { get; private set; }

    public decimal CreditAmount { get; private set; }

    public decimal RunningBalance { get; private set; }

    public CustomerLedger(
        DateTime transactionDate,
        string description,
        decimal debitAmount,
        decimal creditAmount,
        decimal runningBalance)
    {
        Id = Guid.NewGuid();
        TransactionDate = transactionDate;
        Description = description;
        DebitAmount = debitAmount;
        CreditAmount = creditAmount;
        RunningBalance = runningBalance;
    }

    public void UpdateRunningBalance(decimal balance)
    {
        RunningBalance = balance;
    }
}
