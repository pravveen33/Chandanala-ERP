namespace Chandanala.Core.Identity.Domain.Events;

/// <summary>
/// Base interface for all identity-related domain events.
/// Enables the Event Bus to route all security events to the Governance Fabric.
/// </summary>
public interface IIdentityDomainEvent { DateTime OccurredOn { get; } }

public record UserAuthenticatedEvent(
    Guid UserId, 
    string Username, 
    string IpAddress, 
    DateTime OccurredOn
) : IIdentityDomainEvent;

public record UserAccessDeniedEvent(
    Guid UserId, 
    string RequiredPermission, 
    string Resource, 
    DateTime OccurredOn
) : IIdentityDomainEvent;

public record UserSecurityIncidentDetectedEvent(
    Guid UserId, 
    string IncidentType, 
    string Severity, 
    DateTime OccurredOn
) : IIdentityDomainEvent;