using Chandanala.ERP.Domain.Customers.Entities;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class CustomerLedgerTests
{
    [Fact]
    public void Constructor_Should_Create_Ledger_Entry()
    {
        var ledger = new CustomerLedger(
            DateTime.Today,
            "Invoice #1001",
            500m,
            0m,
            500m);

        Assert.Equal("Invoice #1001", ledger.Description);
        Assert.Equal(500m, ledger.DebitAmount);
        Assert.Equal(0m, ledger.CreditAmount);
        Assert.Equal(500m, ledger.RunningBalance);
    }

    [Fact]
    public void UpdateRunningBalance_Should_Update_Value()
    {
        var ledger = new CustomerLedger(
            DateTime.Today,
            "Invoice",
            500m,
            0m,
            500m);

        ledger.UpdateRunningBalance(250m);

        Assert.Equal(250m, ledger.RunningBalance);
    }
}
