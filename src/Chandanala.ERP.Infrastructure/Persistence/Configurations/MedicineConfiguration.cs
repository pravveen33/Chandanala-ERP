using Chandanala.ERP.Domain.Medicines.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Infrastructure.Persistence.Configurations;

public sealed class MedicineConfiguration : IEntityTypeConfiguration<Medicine>
{
    public void Configure(EntityTypeBuilder<Medicine> builder)
    {
        builder.ToTable("Medicines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MedicineCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.MedicineCode)
            .IsUnique();

        builder.Property(x => x.Barcode)
            .HasMaxLength(20);

        builder.Property(x => x.BrandName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.GenericName)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasMany(x => x.Batches)
            .WithOne()
            .HasForeignKey(x => x.MedicineId);

        builder.Navigation(x => x.Batches)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}