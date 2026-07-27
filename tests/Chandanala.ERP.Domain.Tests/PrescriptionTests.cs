using Chandanala.ERP.Domain.Customers.Entities;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class PrescriptionTests
{
    [Fact]
    public void Constructor_Should_Create_Active_Prescription()
    {
        var prescription = new Prescription(
            "RX-1001",
            "Dr. Sharma",
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 2, 1));

        Assert.Equal(PrescriptionStatus.Active, prescription.Status);
        Assert.Equal("RX-1001", prescription.PrescriptionNumber);
    }

    [Fact]
    public void MarkCompleted_Should_Update_Status()
    {
        var prescription = new Prescription(
            "RX-1002",
            "Dr. Rao",
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 2, 1));

        prescription.MarkCompleted();

        Assert.Equal(PrescriptionStatus.Completed, prescription.Status);
    }

    [Fact]
    public void Cancel_Should_Update_Status()
    {
        var prescription = new Prescription(
            "RX-1003",
            "Dr. Rao",
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 2, 1));

        prescription.Cancel();

        Assert.Equal(PrescriptionStatus.Cancelled, prescription.Status);
    }

    [Fact]
    public void IsExpired_Should_Return_True_When_Date_Has_Passed()
    {
        var prescription = new Prescription(
            "RX-1004",
            "Dr. Rao",
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 31));

        Assert.True(prescription.IsExpired(new DateOnly(2026, 2, 1)));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Expiry_Is_Before_Issue()
    {
        Assert.Throws<ArgumentException>(() =>
            new Prescription(
                "RX-1005",
                "Dr. Rao",
                new DateOnly(2026, 2, 1),
                new DateOnly(2026, 1, 1)));
    }
}
