using Chandanala.ERP.Domain.Medicines.Entities;
using Chandanala.ERP.Infrastructure.Persistence.Configurations;
using Chandanala.ERP.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Infrastructure.Persistence;

public sealed class CatalogDbContext : DbContext, ICatalogDbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Medicine> Medicines => Set<Medicine>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();

    public DbSet<DosageForm> DosageForms => Set<DosageForm>();

    public DbSet<MedicineSchedule> MedicineSchedules => Set<MedicineSchedule>();

    public DbSet<StorageCondition> StorageConditions => Set<StorageCondition>();

    public DbSet<PackType> PackTypes => Set<PackType>();

    public DbSet<UnitOfMeasure> UnitOfMeasures => Set<UnitOfMeasure>();

    public DbSet<MedicineBatch> MedicineBatches => Set<MedicineBatch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MedicineConfiguration).Assembly);
    }
}

