using Chandanala.ERP.Application.Abstractions.Persistence;
using Chandanala.ERP.Application.Medicines.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Application.Medicines.QueryHandlers;

public sealed class GetMedicineByIdQueryHandler
{
    private readonly ICatalogDbContext _catalog;

    public GetMedicineByIdQueryHandler(ICatalogDbContext catalog)
    {
        _catalog = catalog;
    }

    public async Task<MedicineDto?> HandleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _catalog.Medicines
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new MedicineDto
            {
                Id = m.Id,
                MedicineCode = m.MedicineCode,
                BrandName = m.BrandName,
                GenericName = m.GenericName,
                Barcode = m.Barcode,
                RequiresPrescription = m.RequiresPrescription,
                IsActive = m.IsActive,
                IsDiscontinued = m.IsDiscontinued
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
