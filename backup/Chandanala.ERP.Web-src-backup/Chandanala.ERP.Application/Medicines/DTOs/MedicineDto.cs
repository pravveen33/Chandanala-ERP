namespace Chandanala.ERP.Application.Medicines.DTOs;

public sealed class MedicineDto
{
    public Guid Id { get; init; }

    public string MedicineCode { get; init; } = string.Empty;

    public string BrandName { get; init; } = string.Empty;

    public string GenericName { get; init; } = string.Empty;

    public string Barcode { get; init; } = string.Empty;

    public bool RequiresPrescription { get; init; }

    public bool IsActive { get; init; }

    public bool IsDiscontinued { get; init; }
}
