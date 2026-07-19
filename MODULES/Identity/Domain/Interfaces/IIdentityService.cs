namespace Chandanala.Core.Identity.Domain.Interfaces;

/// <summary>
/// Defines the canonical Identity capability contract.
/// All modules depend on this abstraction, ensuring loose coupling 
/// and adherence to ADR-0001.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Verifies if a user has the required permission for a specific business action.
    /// Used by the Governance Fabric for RBAC/ABAC checks.
    /// </summary>
    Task<bool> IsAuthorizedAsync(Guid userId, string permission, CancellationToken ct = default);

    /// <summary>
    /// Retrieves the current user's security context.
    /// Returns the identity state without exposing underlying database models.
    /// </summary>
    Task<UserContext?> GetUserContextAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Validates the authenticity of an identity token.
    /// </summary>
    Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default);
}

/// <summary>
/// Immutable record representing the user context within the system.
/// </summary>
public record UserContext(
    Guid UserId, 
    string Username, 
    string[] Roles, 
    Dictionary<string, string> Claims
);