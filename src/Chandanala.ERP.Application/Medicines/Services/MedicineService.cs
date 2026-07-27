using Chandanala.ERP.Application.Abstractions.Persistence;
using Chandanala.ERP.Application.Medicines.DTOs;
using Chandanala.ERP.Domain.Medicines.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chandanala.ERP.Application.Medicines.Services;

public sealed class MedicineService : IMedicineService
{
    private readonly ICatalogDbContext _db;

    public MedicineService(ICatalogDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<MedicineDto>> GetMedicinesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _db.Medicines
            .AsNoTracking()
            .OrderBy(x => x.BrandName)
            .Select(Map)
            .ToListAsync(cancellationToken);
    }

    public async Task<MedicineDto?> GetMedicineByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _db.Medicines
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(Map)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<MedicineDto>> SearchAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetMedicinesAsync(cancellationToken);

        searchTerm = searchTerm.Trim();

        return await _db.Medicines
            .AsNoTracking()
            .Where(x =>
                x.BrandName.Contains(searchTerm) ||
                x.GenericName.Contains(searchTerm) ||
                x.MedicineCode.Contains(searchTerm))
            .OrderBy(x => x.BrandName)
            .Select(Map)
            .ToListAsync(cancellationToken);
    }

    public async Task<Guid> CreateAsync(
        string medicineCode,
        string brandName,
        string genericName,
        CancellationToken cancellationToken = default)
    {
        var medicine = Medicine.Create(
            medicineCode,
            brandName,
            genericName);

        _db.Medicines.Add(medicine);

        await _db.SaveChangesAsync(cancellationToken);

        return medicine.Id;
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        string brandName,
        CancellationToken cancellationToken = default)
    {
        var medicine = await _db.Medicines
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (medicine is null)
            return false;

        medicine.Rename(brandName);

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var medicine = await _db.Medicines
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (medicine is null)
            return false;

        medicine.Discontinue();

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static readonly Expression<Func<Medicine, MedicineDto>> Map =
        medicine => new MedicineDto
        {
            Id = medicine.Id,
            MedicineCode = medicine.MedicineCode,
            BrandName = medicine.BrandName,
            GenericName = medicine.GenericName,
            Barcode = medicine.Barcode,
            RequiresPrescription = medicine.RequiresPrescription,
            IsDiscontinued = medicine.IsDiscontinued
        };
}
