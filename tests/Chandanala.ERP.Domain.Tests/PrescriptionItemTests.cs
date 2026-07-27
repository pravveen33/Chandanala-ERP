using Chandanala.ERP.Domain.Customers.Entities;
using Xunit;

namespace Chandanala.ERP.Domain.Tests;

public class PrescriptionItemTests
{
    [Fact]
    public void Constructor_Should_Create_Item()
    {
        var item = new PrescriptionItem(
            "Paracetamol",
            "500 mg",
            "1 Tablet",
            "Twice Daily",
            5,
            10);

        Assert.Equal("Paracetamol", item.MedicineName);
        Assert.False(item.IsDispensed);
    }

    [Fact]
    public void MarkDispensed_Should_Set_Status()
    {
        var item = new PrescriptionItem(
            "Paracetamol",
            "500 mg",
            "1 Tablet",
            "Twice Daily",
            5,
            10);

        item.MarkDispensed();

        Assert.True(item.IsDispensed);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Quantity_Is_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PrescriptionItem(
                "Paracetamol",
                "500 mg",
                "1 Tablet",
                "Twice Daily",
                5,
                0));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Duration_Is_Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PrescriptionItem(
                "Paracetamol",
                "500 mg",
                "1 Tablet",
                "Twice Daily",
                0,
                10));
    }
}
