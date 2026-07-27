using Chandanala.ERP.Web.Modules.MasterData.DosageForms.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Web.Data.Configurations;

public sealed class DosageFormConfiguration
    : IEntityTypeConfiguration<DosageForm>
{
    public void Configure(EntityTypeBuilder<DosageForm> builder)
    {
        builder.ToTable("DosageForms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasIndex(x => x.Code).IsUnique();

        builder.HasIndex(x => x.Name);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}