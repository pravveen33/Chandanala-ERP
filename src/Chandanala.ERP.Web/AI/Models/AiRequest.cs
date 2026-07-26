namespace Chandanala.ERP.Web.AI.Models;

public sealed class AiRequest
{
    public string Prompt { get; set; } = string.Empty;

    public string? Context { get; set; }

    public string? UserId { get; set; }

    public string? Module { get; set; }
}