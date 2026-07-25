namespace Chandanala.ERP.Web.Shared.MasterData.Entities;

/// <summary>
/// Base entity for all lookup/master tables.
/// </summary>
public abstract class MasterEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }
}
