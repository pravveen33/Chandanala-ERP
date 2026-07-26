using Chandanala.ERP.Domain.Common.Base;

namespace Chandanala.ERP.Domain.Medicines.Entities;

public sealed class Medicine : AuditableEntity
{
    private readonly List<MedicineBatch> _batches = new();

    private Medicine()
    {
    }

    public string MedicineCode { get; private set; } = string.Empty;

    public string Barcode { get; private set; } = string.Empty;

    public string BrandName { get; private set; } = string.Empty;

    public string GenericName { get; private set; } = string.Empty;

    public Guid ManufacturerId { get; private set; }

    public Guid CategoryId { get; private set; }

    public Guid DosageFormId { get; private set; }

    public Guid PackTypeId { get; private set; }

    public Guid UnitOfMeasureId { get; private set; }

    public Guid StorageConditionId { get; private set; }

    public Guid MedicineScheduleId { get; private set; }

    public Guid? RackId { get; private set; }

    public Guid? ShelfId { get; private set; }

    public int MinimumStock { get; private set; }

    public int MaximumStock { get; private set; }

    public int ReorderLevel { get; private set; }

    public bool RequiresPrescription { get; private set; }

    public bool IsActive { get; private set; }

    public bool IsDiscontinued { get; private set; }

    public IReadOnlyCollection<MedicineBatch> Batches => _batches.AsReadOnly();

    public static Medicine Create(
        string medicineCode,
        string brandName,
        string genericName)
    {
        if (string.IsNullOrWhiteSpace(medicineCode))
            throw new ArgumentException("Medicine code is required.");

        if (string.IsNullOrWhiteSpace(brandName))
            throw new ArgumentException("Brand name is required.");

        if (string.IsNullOrWhiteSpace(genericName))
            throw new ArgumentException("Generic name is required.");

        return new Medicine
        {
            MedicineCode = medicineCode.Trim(),
            BrandName = brandName.Trim(),
            GenericName = genericName.Trim(),
            IsActive = true,
            IsDiscontinued = false
        };
    }

    public void Rename(string brandName)
    {
        if (string.IsNullOrWhiteSpace(brandName))
            throw new ArgumentException(nameof(brandName));

        BrandName = brandName.Trim();
    }

    public void ChangeBarcode(string barcode)
    {
        Barcode = barcode?.Trim() ?? string.Empty;
    }

    public void SetStockPolicy(
        int minimum,
        int maximum,
        int reorder)
    {
        if (minimum < 0)
            throw new ArgumentOutOfRangeException(nameof(minimum));

        if (maximum < minimum)
            throw new ArgumentException("Maximum stock cannot be less than minimum stock.");

        if (reorder > maximum)
            throw new ArgumentException("Reorder level cannot exceed maximum stock.");

        MinimumStock = minimum;
        MaximumStock = maximum;
        ReorderLevel = reorder;
    }

    public void AddBatch(MedicineBatch batch)
    {
        ArgumentNullException.ThrowIfNull(batch);

        if (IsDiscontinued)
            throw new InvalidOperationException("Cannot add batches to a discontinued medicine.");

        _batches.Add(batch);
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void Discontinue()
    {
        IsDiscontinued = true;
        IsActive = false;
    }
}
