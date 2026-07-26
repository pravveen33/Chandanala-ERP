using Chandanala.ERP.Application.Abstractions.Persistence;
using Chandanala.ERP.Domain.Medicines.Entities;

namespace Chandanala.ERP.Application.Medicines.CommandHandlers;

public sealed class CreateMedicineCommandHandler
{
    private readonly ICatalogDbContext _catalog;

    public CreateMedicineCommandHandler(ICatalogDbContext catalog)
    {
        _catalog = catalog;
    }

    public async Task<Guid> HandleAsync(
        string medicineCode,
        string brandName,
        string genericName,
        CancellationToken cancellationToken = default)
    {
        var medicine = Medicine.Create(
            medicineCode,
            brandName,
            genericName);

        await _catalog.Medicines.AddAsync(medicine, cancellationToken);

        await _catalog.SaveChangesAsync(cancellationToken);

        return medicine.Id;
    }
}
