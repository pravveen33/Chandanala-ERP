using Chandanala.ERP.Domain.Masters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Infrastructure.Persistence.Configurations;

public sealed class PackTypeConfiguration : IEntityTypeConfiguration<PackType>
{
    public void Configure(EntityTypeBuilder<PackType> builder)
    {
        builder.ToTable("PackTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();


        builder.HasIndex(x => x.Code).IsUnique();
    }
}
