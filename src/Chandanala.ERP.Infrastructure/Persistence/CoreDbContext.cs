using Chandanala.ERP.Domain.Customers.Entities;
using Chandanala.ERP.Domain.MedicineKnowledge.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Infrastructure.Persistence;

public sealed class CoreDbContext : DbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<MedicineKnowledge> MedicineKnowledge => Set<MedicineKnowledge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
    }
}
