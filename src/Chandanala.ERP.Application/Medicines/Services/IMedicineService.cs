using Chandanala.ERP.Application.Medicines.DTOs;

namespace Chandanala.ERP.Application.Medicines.Services;

public interface IMedicineService
{
    Task<IReadOnlyList<MedicineDto>> GetMedicinesAsync(
        CancellationToken cancellationToken = default);

    Task<MedicineDto?> GetMedicineByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MedicineDto>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateAsync(
        string medicineCode,
        string brandName,
        string genericName,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        Guid id,
        string brandName,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
