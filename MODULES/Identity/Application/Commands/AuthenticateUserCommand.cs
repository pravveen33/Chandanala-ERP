namespace Chandanala.Core.Identity.Application.Commands;

/// <summary>
/// Command to initiate user authentication.
/// The Handler will validate credentials and publish a UserAuthenticatedEvent.
/// </summary>
public record AuthenticateUserCommand(string Username, string Password, string IpAddress);