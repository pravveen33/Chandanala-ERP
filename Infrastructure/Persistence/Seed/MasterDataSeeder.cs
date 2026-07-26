using Chandanala.ERP.Domain.Masters.Entities;
using Chandanala.ERP.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Infrastructure.Persistence.Seed;

public static class MasterDataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.PackTypes.AnyAsync())
        {
            db.PackTypes.AddRange(
                new PackType { Code = "STRIP", Name = "Strip", DisplayOrder = 1 },
                new PackType { Code = "BOTTLE", Name = "Bottle", DisplayOrder = 2 },
                new PackType { Code = "TUBE", Name = "Tube", DisplayOrder = 3 },
                new PackType { Code = "VIAL", Name = "Vial", DisplayOrder = 4 },
                new PackType { Code = "AMPOULE", Name = "Ampoule", DisplayOrder = 5 },
                new PackType { Code = "SACHET", Name = "Sachet", DisplayOrder = 6 },
                new PackType { Code = "JAR", Name = "Jar", DisplayOrder = 7 },
                new PackType { Code = "POUCH", Name = "Pouch", DisplayOrder = 8 },
                new PackType { Code = "INHALER", Name = "Inhaler", DisplayOrder = 9 },
                new PackType { Code = "SYRINGE", Name = "Syringe", DisplayOrder = 10 }
            );
        }

        if (!await db.UnitOfMeasures.AnyAsync())
        {
            db.UnitOfMeasures.AddRange(
                new UnitOfMeasure { Code = "TAB", Name = "Tablet", Symbol = "Tab", DisplayOrder = 1 },
                new UnitOfMeasure { Code = "CAP", Name = "Capsule", Symbol = "Cap", DisplayOrder = 2 },
                new UnitOfMeasure { Code = "ML", Name = "Millilitre", Symbol = "mL", DisplayOrder = 3 },
                new UnitOfMeasure { Code = "G", Name = "Gram", Symbol = "g", DisplayOrder = 4 },
                new UnitOfMeasure { Code = "MG", Name = "Milligram", Symbol = "mg", DisplayOrder = 5 },
                new UnitOfMeasure { Code = "PCS", Name = "Piece", Symbol = "Pc", DisplayOrder = 6 },
                new UnitOfMeasure { Code = "DOSE", Name = "Dose", Symbol = "Dose", DisplayOrder = 7 },
                new UnitOfMeasure { Code = "IU", Name = "International Unit", Symbol = "IU", DisplayOrder = 8 }
            );
        }

        await db.SaveChangesAsync();
    }
}

