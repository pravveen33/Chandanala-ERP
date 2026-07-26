using Chandanala.ERP.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Application.Medicines.CommandHandlers;

public sealed class DeleteMedicineCommandHandler
{
    private readonly ICatalogDbContext _catalog;

    public DeleteMedicineCommandHandler(ICatalogDbContext catalog)
    {
        _catalog = catalog;
    }

    public async Task<bool> HandleAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var medicine = await _catalog.Medicines
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        if (medicine is null)
            return false;

        _catalog.Medicines.Remove(medicine);

        await _catalog.SaveChangesAsync(cancellationToken);

        return true;
    }
}
