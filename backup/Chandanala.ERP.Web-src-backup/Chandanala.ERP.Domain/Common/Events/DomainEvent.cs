namespace Chandanala.ERP.Domain.Common.Events;

public abstract record DomainEvent
{
    public Guid EventId { get; init; } = Guid.CreateVersion7();

    public DateTime OccurredUtc { get; init; } = DateTime.UtcNow;

    public Guid? CorrelationId { get; init; }

    public Guid? CausationId { get; init; }
}
