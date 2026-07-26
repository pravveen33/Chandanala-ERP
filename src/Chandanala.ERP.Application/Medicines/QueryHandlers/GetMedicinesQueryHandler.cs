using Chandanala.ERP.Application.Abstractions.Persistence;
using Chandanala.ERP.Application.Medicines.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Application.Medicines.QueryHandlers;

public sealed class GetMedicinesQueryHandler
{
    private readonly ICatalogDbContext _catalog;

    public GetMedicinesQueryHandler(
        ICatalogDbContext catalog)
    {
        _catalog = catalog;
    }

    public async Task<IReadOnlyList<MedicineDto>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        return await _catalog.Medicines
            .AsNoTracking()
            .OrderBy(m => m.BrandName)
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
            .ToListAsync(cancellationToken);
    }
}
