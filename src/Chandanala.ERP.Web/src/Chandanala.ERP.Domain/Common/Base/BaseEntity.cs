using System.ComponentModel.DataAnnotations.Schema;

using Chandanala.ERP.Domain.Common.Events;

namespace Chandanala.ERP.Domain.Common.Base;

public abstract class BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = new();

    public Guid Id { get; protected set; } = Guid.CreateVersion7();

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
