namespace Chandanala.ERP.Domain.MedicineKnowledge.Events;

public sealed record MedicineUpdatedEvent(
    Guid MedicineId,
    DateTime UpdatedOnUtc
);
