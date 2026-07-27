using Chandanala.ERP.Domain.MedicineKnowledge.Entities;
using Chandanala.ERP.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Infrastructure.Persistence;

public sealed class CoreDbContext : DbContext
{
    public CoreDbContext(DbContextOptions<CoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<MedicineKnowledge> MedicineKnowledge => Set<MedicineKnowledge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicitly register ONLY MedicineKnowledge
        modelBuilder.ApplyConfiguration(new MedicineKnowledgeConfiguration());
    }
}
