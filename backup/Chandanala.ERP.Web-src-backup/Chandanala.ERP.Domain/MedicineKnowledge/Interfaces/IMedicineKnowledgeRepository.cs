using MedicineKnowledgeEntity = Chandanala.ERP.Domain.MedicineKnowledge.Entities.MedicineKnowledge;

namespace Chandanala.ERP.Domain.MedicineKnowledge.Interfaces;

public interface IMedicineKnowledgeRepository
{
    Task<MedicineKnowledgeEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<MedicineKnowledgeEntity?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        MedicineKnowledgeEntity medicine,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        MedicineKnowledgeEntity medicine,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        MedicineKnowledgeEntity medicine,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}