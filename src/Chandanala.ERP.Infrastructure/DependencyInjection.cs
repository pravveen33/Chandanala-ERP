using Chandanala.ERP.Application.Medicines.CommandHandlers;
using Chandanala.ERP.Application.Medicines.QueryHandlers;
using Chandanala.ERP.Application.Medicines.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Chandanala.ERP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        // Query handlers
        services.AddScoped<GetMedicinesQueryHandler>();
        services.AddScoped<GetMedicineByIdQueryHandler>();
        services.AddScoped<SearchMedicineQueryHandler>();

        // Command handlers
        services.AddScoped<CreateMedicineCommandHandler>();
        services.AddScoped<UpdateMedicineCommandHandler>();
        services.AddScoped<DeleteMedicineCommandHandler>();

        // Service
        services.AddScoped<
            IMedicineService,
            Chandanala.ERP.Infrastructure.Medicines.Services.MedicineService>();

        return services;
    }
}