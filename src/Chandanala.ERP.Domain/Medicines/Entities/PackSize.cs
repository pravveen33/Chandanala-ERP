namespace Chandanala.ERP.Domain.Medicines.Entities;

public sealed class PackSize
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }
}
