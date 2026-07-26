using Chandanala.ERP.Domain.Masters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Infrastructure.Persistence.Configurations;

public sealed class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.ToTable("UnitOfMeasures");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Symbol)
               .HasMaxLength(20);

        builder.HasIndex(x => x.Code).IsUnique();
    }
}
