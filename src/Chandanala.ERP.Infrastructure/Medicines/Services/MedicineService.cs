using Chandanala.ERP.Application.Medicines.CommandHandlers;
using Chandanala.ERP.Application.Medicines.DTOs;
using Chandanala.ERP.Application.Medicines.QueryHandlers;
using Chandanala.ERP.Application.Medicines.Services;

namespace Chandanala.ERP.Infrastructure.Medicines.Services;

public sealed class MedicineService : IMedicineService
{
    private readonly GetMedicinesQueryHandler _getMedicines;
    private readonly GetMedicineByIdQueryHandler _getMedicineById;
    private readonly SearchMedicineQueryHandler _searchMedicine;
    private readonly CreateMedicineCommandHandler _createMedicine;
    private readonly UpdateMedicineCommandHandler _updateMedicine;
    private readonly DeleteMedicineCommandHandler _deleteMedicine;

    public MedicineService(
        GetMedicinesQueryHandler getMedicines,
        GetMedicineByIdQueryHandler getMedicineById,
        SearchMedicineQueryHandler searchMedicine,
        CreateMedicineCommandHandler createMedicine,
        UpdateMedicineCommandHandler updateMedicine,
        DeleteMedicineCommandHandler deleteMedicine)
    {
        _getMedicines = getMedicines;
        _getMedicineById = getMedicineById;
        _searchMedicine = searchMedicine;
        _createMedicine = createMedicine;
        _updateMedicine = updateMedicine;
        _deleteMedicine = deleteMedicine;
    }

    public Task<IReadOnlyList<MedicineDto>> GetMedicinesAsync(
        CancellationToken cancellationToken = default)
        => _getMedicines.HandleAsync(cancellationToken);

    public Task<MedicineDto?> GetMedicineByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _getMedicineById.HandleAsync(id, cancellationToken);

    public Task<IReadOnlyList<MedicineDto>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
        => _searchMedicine.HandleAsync(searchTerm, cancellationToken);

    public Task<Guid> CreateAsync(
        string medicineCode,
        string brandName,
        string genericName,
        CancellationToken cancellationToken = default)
        => _createMedicine.HandleAsync(
            medicineCode,
            brandName,
            genericName,
            cancellationToken);

    public Task<bool> UpdateAsync(
        Guid id,
        string brandName,
        CancellationToken cancellationToken = default)
        => _updateMedicine.HandleAsync(
            id,
            brandName,
            cancellationToken);

    public Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => _deleteMedicine.HandleAsync(
            id,
            cancellationToken);
}