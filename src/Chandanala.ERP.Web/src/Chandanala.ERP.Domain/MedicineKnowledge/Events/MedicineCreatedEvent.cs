using Chandanala.ERP.Domain.MedicineKnowledge.Entities;

namespace Chandanala.ERP.Domain.MedicineKnowledge.Events;

public sealed record MedicineCreatedEvent(
    Guid MedicineId,
    string Code,
    string BrandName
);
