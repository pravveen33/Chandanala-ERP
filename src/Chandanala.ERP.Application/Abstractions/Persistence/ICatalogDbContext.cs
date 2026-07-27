using Chandanala.ERP.Domain.Medicines.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Application.Abstractions.Persistence;

public interface ICatalogDbContext
{
    DbSet<Medicine> Medicines { get; }

    DbSet<Category> Categories { get; }

    DbSet<Manufacturer> Manufacturers { get; }

    DbSet<DosageForm> DosageForms { get; }

    DbSet<MedicineBatch> MedicineBatches { get; }

    DbSet<PackType> PackTypes { get; }

    DbSet<PackSize> PackSizes { get; }

    DbSet<UnitOfMeasure> UnitOfMeasures { get; }

    DbSet<MedicineSchedule> MedicineSchedules { get; }

    DbSet<StorageCondition> StorageConditions { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}

