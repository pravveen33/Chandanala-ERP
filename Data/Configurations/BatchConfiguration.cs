using Chandanala.ERP.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chandanala.ERP.Web.Data.Configurations;

public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> entity)
    {
        // EF Core configuration
    }
}