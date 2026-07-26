namespace Chandanala.ERP.Domain.Common.Base;

public abstract class AuditableEntity : AggregateRoot
{
    public DateTime CreatedUtc { get; protected set; }

    public string? CreatedBy { get; protected set; }

    public DateTime? LastModifiedUtc { get; protected set; }

    public string? LastModifiedBy { get; protected set; }
}
