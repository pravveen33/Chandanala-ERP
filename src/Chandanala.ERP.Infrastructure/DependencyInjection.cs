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
    {
        // Medicine Query Handlers
        services.AddScoped<GetMedicinesQueryHandler>();
        services.AddScoped<GetMedicineByIdQueryHandler>();
        services.AddScoped<SearchMedicineQueryHandler>();

        // Medicine Command Handlers
        services.AddScoped<CreateMedicineCommandHandler>();
        services.AddScoped<UpdateMedicineCommandHandler>();
        services.AddScoped<DeleteMedicineCommandHandler>();

        services.AddScoped<IMedicineService, MedicineService>();

        return services;
    }
}

