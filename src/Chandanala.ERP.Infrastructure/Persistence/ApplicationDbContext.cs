using Chandanala.ERP.Domain.MedicineKnowledge.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MedicineKnowledge> MedicineKnowledge => Set<MedicineKnowledge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}
