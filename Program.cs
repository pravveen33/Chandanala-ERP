using Chandanala.ERP.Application.Abstractions.Persistence;
using Chandanala.ERP.Infrastructure.Persistence;
using Chandanala.ERP.Infrastructure.Persistence.Seed;
using Chandanala.ERP.Domain.MedicineKnowledge.Interfaces;
using Chandanala.ERP.Infrastructure.Persistence.Repositories;
using Chandanala.ERP.Web.Components;
using Chandanala.ERP.Web.Data;
using Chandanala.ERP.Web.Services;
using Chandanala.ERP.Web.Modules.Procurement.SupplierMaster.Repositories;
using Chandanala.ERP.Web.Modules.Procurement.SupplierMaster.Services;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

// ======================================================
// Database Configuration
// ======================================================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<CoreDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICatalogDbContext>(
    sp => sp.GetRequiredService<CatalogDbContext>());

// ======================================================
// Core ERP Services
// ======================================================

builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<VerificationService>();

// ======================================================
// Procurement Module Services
// ======================================================

builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierMasterService, SupplierMasterService>();

// ======================================================
// Medicine Knowledge Module Services
// ======================================================

builder.Services.AddScoped<IMedicineKnowledgeRepository, MedicineKnowledgeRepository>();

// ======================================================
// Engineering Services
// ======================================================

// Register EngineeringDashboardService here when implemented.
// Example:
// builder.Services.AddScoped<EngineeringDashboardService>();

// ======================================================
// Razor Components
// ======================================================

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await MasterDataSeeder.SeedAsync(dbContext);
}

// ======================================================
// HTTP Pipeline
// ======================================================

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();





