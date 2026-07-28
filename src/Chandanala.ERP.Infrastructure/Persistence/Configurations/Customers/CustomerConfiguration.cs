using Chandanala.ERP.Domain.Customers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Infrastructure.Persistence.Configurations.Customers;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CustomerCode)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.CustomerCode)
            .IsUnique();

        builder.Property(x => x.FullName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.OwnsOne(x => x.PhoneNumber, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(256);
        });

        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.Line1)
                .HasColumnName("AddressLine1")
                .HasMaxLength(200);

            address.Property(a => a.Line2)
                .HasColumnName("AddressLine2")
                .HasMaxLength(200);

            address.Property(a => a.City)
                .HasColumnName("City")
                .HasMaxLength(100);

            address.Property(a => a.State)
                .HasColumnName("State")
                .HasMaxLength(100);

            address.Property(a => a.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(20);

            address.Property(a => a.Country)
                .HasColumnName("Country")
                .HasMaxLength(100);
        });
    }
}
