using Chandanala.ERP.Web.Components;
using Chandanala.ERP.Web.Data;
using Chandanala.ERP.Web.Services;
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

// ======================================================
// Core ERP Services
// ======================================================

builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<VerificationService>();

// ======================================================
// Procurement Module Services
// ======================================================

builder.Services.AddScoped<SupplierMasterService>();

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
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();