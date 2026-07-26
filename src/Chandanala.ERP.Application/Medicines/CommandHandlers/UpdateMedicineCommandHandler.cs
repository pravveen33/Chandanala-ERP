using Chandanala.ERP.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Application.Medicines.CommandHandlers;

public sealed class UpdateMedicineCommandHandler
{
    private readonly ICatalogDbContext _catalog;

    public UpdateMedicineCommandHandler(ICatalogDbContext catalog)
    {
        _catalog = catalog;
    }

    public async Task<bool> HandleAsync(
        Guid id,
        string brandName,
        CancellationToken cancellationToken = default)
    {
        var medicine = await _catalog.Medicines
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (medicine is null)
            return false;

        medicine.Rename(brandName);

        await _catalog.SaveChangesAsync(cancellationToken);

        return true;
    }
}
