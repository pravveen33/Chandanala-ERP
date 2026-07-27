using System.Text.RegularExpressions;

namespace Chandanala.ERP.Domain.Common.ValueObjects;

public sealed record EmailAddress
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required.", nameof(value));

        value = value.Trim();

        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException("Invalid email address.", nameof(value));

        Value = value.ToLowerInvariant();
    }

    public override string ToString() => Value;

    public static implicit operator string(EmailAddress email)
        => email.Value;
}
