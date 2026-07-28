using Chandanala.ERP.Domain.MedicineKnowledge.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Infrastructure.Persistence.Configurations;

public sealed class MedicineKnowledgeConfiguration
    : IEntityTypeConfiguration<MedicineKnowledge>
{
    public void Configure(EntityTypeBuilder<MedicineKnowledge> builder)
    {
        builder.ToTable("MedicineKnowledge");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.BrandName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.GenericName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Strength)
            .HasMaxLength(50);

        builder.Property(x => x.DosageForm)
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .HasConversion<string>();

        builder.Property(x => x.DrugSchedule)
            .HasConversion<string>();

        builder.Property(x => x.StorageType)
            .HasConversion<string>();

        builder.OwnsOne(x => x.Barcode, barcode =>
        {
            barcode.Property(b => b.Value)
                .HasColumnName("Barcode")
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.HasIndex(x => x.Code)
            .IsUnique();
builder.HasQueryFilter(x => !x.IsDeleted);
    }
}


