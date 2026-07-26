namespace Chandanala.ERP.Domain.Masters.Entities;

public sealed class UnitOfMeasure
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Symbol { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }
}
