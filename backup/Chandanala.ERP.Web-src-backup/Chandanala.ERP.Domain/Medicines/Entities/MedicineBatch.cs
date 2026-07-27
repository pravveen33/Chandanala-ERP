using Chandanala.ERP.Domain.Common.Base;

namespace Chandanala.ERP.Domain.Medicines.Entities;

public sealed class MedicineBatch : AuditableEntity
{
    private MedicineBatch()
    {
    }

    public Guid MedicineId { get; private set; }

    public string BatchNumber { get; private set; } = string.Empty;

    public DateOnly ManufacturingDate { get; private set; }

    public DateOnly ExpiryDate { get; private set; }

    public decimal PurchasePrice { get; private set; }

    public decimal SellingPrice { get; private set; }

    public decimal Mrp { get; private set; }

    public int QuantityInStock { get; private set; }

    public static MedicineBatch Create(
        Guid medicineId,
        string batchNumber,
        DateOnly manufacturingDate,
        DateOnly expiryDate)
    {
        if (string.IsNullOrWhiteSpace(batchNumber))
            throw new ArgumentException("Batch number is required.");

        if (expiryDate <= manufacturingDate)
            throw new ArgumentException("Expiry date must be after manufacturing date.");

        return new MedicineBatch
        {
            MedicineId = medicineId,
            BatchNumber = batchNumber.Trim(),
            ManufacturingDate = manufacturingDate,
            ExpiryDate = expiryDate
        };
    }

    public void UpdatePrices(
        decimal purchasePrice,
        decimal sellingPrice,
        decimal mrp)
    {
        PurchasePrice = purchasePrice;
        SellingPrice = sellingPrice;
        Mrp = mrp;
    }

    public void ReceiveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        QuantityInStock += quantity;
    }

    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        if (QuantityInStock < quantity)
            throw new InvalidOperationException("Insufficient stock.");

        QuantityInStock -= quantity;
    }
}
