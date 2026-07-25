using Chandanala.ERP.Domain.MedicineKnowledge.Enums;
using Chandanala.ERP.Domain.MedicineKnowledge.ValueObjects;

namespace Chandanala.ERP.Domain.MedicineKnowledge.Entities;

public sealed class MedicineKnowledge
{
    public Guid Id { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string BrandName { get; private set; } = string.Empty;

    public string GenericName { get; private set; } = string.Empty;

    public string Strength { get; private set; } = string.Empty;

    public string DosageForm { get; private set; } = string.Empty;

    public Barcode Barcode { get; private set; }

    public MedicineStatus Status { get; private set; }

    public DrugSchedule DrugSchedule { get; private set; }

    public StorageType StorageType { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    private MedicineKnowledge()
    {
    }

    public static MedicineKnowledge Create(
        string code,
        string brandName,
        string genericName,
        string strength,
        string dosageForm,
        Barcode barcode,
        DrugSchedule drugSchedule,
        StorageType storageType)
    {
        return new MedicineKnowledge
        {
            Id = Guid.CreateVersion7(),
            Code = code.Trim(),
            BrandName = brandName.Trim(),
            GenericName = genericName.Trim(),
            Strength = strength.Trim(),
            DosageForm = dosageForm.Trim(),
            Barcode = barcode,
            DrugSchedule = drugSchedule,
            StorageType = storageType,
            Status = MedicineStatus.Active,
            CreatedOnUtc = DateTime.UtcNow
        };
    }

    public void Update(
        string brandName,
        string genericName,
        string strength,
        string dosageForm,
        DrugSchedule drugSchedule,
        StorageType storageType)
    {
        BrandName = brandName.Trim();
        GenericName = genericName.Trim();
        Strength = strength.Trim();
        DosageForm = dosageForm.Trim();
        DrugSchedule = drugSchedule;
        StorageType = storageType;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Archive()
    {
        Status = MedicineStatus.Archived;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}
