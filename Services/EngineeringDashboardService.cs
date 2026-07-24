using System.Text.Json;
using Chandanala.ERP.Web.Data;

namespace Chandanala.ERP.Web.Services;

public class EngineeringDashboardService
{
    private readonly IWebHostEnvironment _environment;

    public EngineeringDashboardService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<EngineeringDashboard?> GetDashboardAsync()
    {
        var filePath = Path.Combine(
            _environment.WebRootPath,
            "data",
            "engineering-dashboard.json");

        if (!File.Exists(filePath))
            return null;

        var json = await File.ReadAllTextAsync(filePath);

        return JsonSerializer.Deserialize<EngineeringDashboard>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
}