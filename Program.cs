using Chandanala.ERP.Web.Components;
using Chandanala.ERP.Web.Data;
using Chandanala.ERP.Web.Services;
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

// ======================================================
// Core ERP Services
// ======================================================

builder.Services.AddScoped<Chandanala.ERP.Web.Services.SupplierService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<VerificationService>();

// ======================================================
// Engineering Services
// ======================================================

// Register EngineeringDashboardService here when the class exists.
// Example:
// builder.Services.AddScoped<EngineeringDashboardService>();

// ======================================================
// Procurement Module Services
// ======================================================

// Procurement services will be registered here when implemented.

// ======================================================
// Razor Components
// ======================================================

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

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

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();