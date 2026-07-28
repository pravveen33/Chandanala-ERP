using Chandanala.ERP.Application.Common.Interfaces;
using Chandanala.ERP.Application.Common.Interfaces.Persistence;
using Chandanala.ERP.Infrastructure.Persistence;
using Chandanala.ERP.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Chandanala.ERP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
