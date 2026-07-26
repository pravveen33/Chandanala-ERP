using System.Collections.Generic;

namespace Chandanala.ERP.AI.Runtime;

public sealed class RuntimeOptions
{
    public string Name { get; set; } = "CHANDANALA CORE AI Runtime";

    public string Version { get; set; } = "1.0";

    public bool EnableAudit { get; set; } = true;

    public bool EnableHealthChecks { get; set; } = true;

    public bool EnableTelemetry { get; set; } = true;

    public bool EnableContext { get; set; } = true;

    public bool ValidateConfigurationOnStartup { get; set; } = true;

    public int MaxConcurrentRequests { get; set; } = 10;

    public int RequestTimeoutSeconds { get; set; } = 30;

    public List<string> ConfigurationFiles { get; set; } = new()
    {
        "github.yaml",
        "filesystem.yaml",
        "postgres.yaml",
        "openapi.yaml",
        "capabilities.yaml",
        "routing.yaml",
        "approval-policy.yaml",
        "audit.yaml",
        "context.yaml",
        "servers.yaml",
        "providers.yaml",
        "models.yaml",
        "health.yaml"
    };
}