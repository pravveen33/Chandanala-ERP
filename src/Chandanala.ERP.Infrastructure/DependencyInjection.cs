using Chandanala.ERP.Application.Medicines.CommandHandlers;
using Chandanala.ERP.Application.Medicines.QueryHandlers;
using Microsoft.Extensions.DependencyInjection;
using Chandanala.ERP.Application.Medicines.Services;
using Chandanala.ERP.Infrastructure.Medicines.Services;
namespace Chandanala.ERP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {        services.AddScoped<IMedicineService, MedicineService>();

        return services;
    }
}


