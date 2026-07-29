using MedicineKnowledgeEntity = Chandanala.ERP.Domain.MedicineKnowledge.Entities.MedicineKnowledge;
using Chandanala.ERP.Domain.MedicineKnowledge.Interfaces;
using Chandanala.ERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chandanala.ERP.Infrastructure.Persistence.Repositories;

public sealed class MedicineKnowledgeRepository
    : IMedicineKnowledgeRepository
{
    private readonly CoreDbContext _context;

    public MedicineKnowledgeRepository(CoreDbContext context)
    {
        _context = context;
    }

    public async Task<MedicineKnowledgeEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.MedicineKnowledge
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<MedicineKnowledgeEntity?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _context.MedicineKnowledge
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public async Task AddAsync(
        MedicineKnowledgeEntity medicine,
        CancellationToken cancellationToken = default)
    {
        await _context.MedicineKnowledge.AddAsync(medicine, cancellationToken);
    }

    public Task UpdateAsync(
        MedicineKnowledgeEntity medicine,
        CancellationToken cancellationToken = default)
    {
        _context.MedicineKnowledge.Update(medicine);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        MedicineKnowledgeEntity medicine,
        CancellationToken cancellationToken = default)
    {
        _context.MedicineKnowledge.Remove(medicine);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
