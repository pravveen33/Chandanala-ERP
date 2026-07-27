namespace Chandanala.ERP.Domain.Common.ValueObjects;

public sealed record Address
{
    public string Line1 { get; }
    public string? Line2 { get; }
    public string City { get; }
    public string State { get; }
    public string PostalCode { get; }
    public string Country { get; }

    public Address(
        string line1,
        string? line2,
        string city,
        string state,
        string postalCode,
        string country = "India")
    {
        if (string.IsNullOrWhiteSpace(line1))
            throw new ArgumentException("Address Line 1 is required.", nameof(line1));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.", nameof(city));

        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State is required.", nameof(state));

        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal Code is required.", nameof(postalCode));

        Line1 = line1.Trim();
        Line2 = line2?.Trim();
        City = city.Trim();
        State = state.Trim();
        PostalCode = postalCode.Trim();
        Country = country.Trim();
    }

    public override string ToString()
        => $"{Line1}, {City}, {State} {PostalCode}, {Country}";
}
