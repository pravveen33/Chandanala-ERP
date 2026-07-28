namespace Chandanala.ERP.Domain.MedicineKnowledge.ValueObjects;

public sealed record Barcode
{
    // Required by EF Core
    private Barcode()
    {
        Value = string.Empty;
    }
    public string Value { get; }

    private Barcode(string value)
    {
        Value = value;
    }

    public static Barcode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Barcode cannot be empty.", nameof(value));

        value = value.Trim();

        if (value.Length < 8 || value.Length > 20)
            throw new ArgumentException("Barcode must be between 8 and 20 characters.", nameof(value));

        if (!value.All(char.IsDigit))
            throw new ArgumentException("Barcode must contain only digits.", nameof(value));

        return new Barcode(value);
    }

    public override string ToString() => Value;
}

