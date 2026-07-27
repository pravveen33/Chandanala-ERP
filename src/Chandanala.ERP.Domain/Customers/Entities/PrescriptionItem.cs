namespace Chandanala.ERP.Domain.Customers.Entities;

public class PrescriptionItem
{
    public Guid Id { get; private set; }

    public string MedicineName { get; private set; }

    public string Strength { get; private set; }

    public string Dosage { get; private set; }

    public string Frequency { get; private set; }

    public int DurationInDays { get; private set; }

    public int Quantity { get; private set; }

    public bool IsDispensed { get; private set; }

    public PrescriptionItem(
        string medicineName,
        string strength,
        string dosage,
        string frequency,
        int durationInDays,
        int quantity)
    {
        if (string.IsNullOrWhiteSpace(medicineName))
            throw new ArgumentException("Medicine name is required.", nameof(medicineName));

        if (durationInDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(durationInDays));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Id = Guid.NewGuid();
        MedicineName = medicineName;
        Strength = strength;
        Dosage = dosage;
        Frequency = frequency;
        DurationInDays = durationInDays;
        Quantity = quantity;
        IsDispensed = false;
    }

    public void MarkDispensed()
    {
        IsDispensed = true;
    }
}
