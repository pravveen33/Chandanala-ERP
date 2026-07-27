using Chandanala.ERP.Domain.Medicines.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Infrastructure.Persistence.Configurations;

public sealed class MedicineScheduleConfiguration : IEntityTypeConfiguration<MedicineSchedule>
{
    public void Configure(EntityTypeBuilder<MedicineSchedule> builder)
    {
        builder.ToTable("MedicineSchedule");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Code)
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}
