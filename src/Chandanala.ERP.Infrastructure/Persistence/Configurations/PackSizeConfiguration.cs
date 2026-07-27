using Chandanala.ERP.Domain.Medicines.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Infrastructure.Persistence.Configurations;

public sealed class PackSizeConfiguration : IEntityTypeConfiguration<PackSize>
{
    public void Configure(EntityTypeBuilder<PackSize> builder)
    {
        builder.ToTable("PackSizes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.DisplayName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Unit)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasPrecision(10,2);
    }
}
