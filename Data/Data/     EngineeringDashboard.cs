namespace Chandanala.ERP.Web.Data;

public class EngineeringDashboard
{
    public DateTime GeneratedAt { get; set; }

    public string Version { get; set; } = "";

    public string Environment { get; set; } = "";

    public BuildInfo Build { get; set; } = new();

    public TestInfo Tests { get; set; } = new();

    public SecurityInfo Security { get; set; } = new();

    public ArchitectureInfo Architecture { get; set; } = new();

    public QualityInfo Quality { get; set; } = new();

    public ReleaseInfo Release { get; set; } = new();
}

public class BuildInfo
{
    public string Status { get; set; } = "";
    public string Duration { get; set; } = "";
    public DateTime LastBuild { get; set; }
}

public class TestInfo
{
    public string Status { get; set; } = "";
    public int Total { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public double Coverage { get; set; }
}

public class SecurityInfo
{
    public string CodeQL { get; set; } = "";
    public string Trivy { get; set; } = "";
    public string Gitleaks { get; set; } = "";
    public string Sbom { get; set; } = "";
}

public class ArchitectureInfo
{
    public string Status { get; set; } = "";
    public int Violations { get; set; }
}

public class QualityInfo
{
    public string Documentation { get; set; } = "";
    public string TechnicalDebt { get; set; } = "";
}

public class ReleaseInfo
{
    public bool Ready { get; set; }
    public bool ApprovalRequired { get; set; }
}