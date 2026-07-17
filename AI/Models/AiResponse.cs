namespace Chandanala.ERP.Web.AI.Models;

public sealed class AiResponse
{
    public bool Success { get; set; }

    public string Response { get; set; } = string.Empty;

    public string Provider { get; set; } = string.Empty;

    public string? Error { get; set; }
}
