using System.Text.RegularExpressions;

namespace Chandanala.ERP.Domain.Common.ValueObjects;

public sealed record PhoneNumber
{
    private static readonly Regex PhoneRegex =
        new(@"^[6-9]\d{9}$", RegexOptions.Compiled);

    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number is required.", nameof(value));

        value = value.Trim();

        if (!PhoneRegex.IsMatch(value))
            throw new ArgumentException("Invalid Indian mobile number.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phone)
        => phone.Value;
}
