using Chandanala.ERP.Application.Abstractions.Persistence;
using Chandanala.ERP.Application.Medicines.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Application.Medicines.QueryHandlers;

public sealed class SearchMedicineQueryHandler
{
    private readonly ICatalogDbContext _catalog;

    public SearchMedicineQueryHandler(ICatalogDbContext catalog)
    {
        _catalog = catalog;
    }

    public async Task<IReadOnlyList<MedicineDto>> HandleAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        searchTerm = (searchTerm ?? string.Empty).Trim();

        return await _catalog.Medicines
            .AsNoTracking()
            .Where(m =>
                m.MedicineCode.Contains(searchTerm) ||
                m.BrandName.Contains(searchTerm) ||
                m.GenericName.Contains(searchTerm))
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
