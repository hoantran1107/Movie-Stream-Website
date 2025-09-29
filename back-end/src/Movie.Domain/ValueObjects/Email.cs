using System.Text.RegularExpressions;
using Movie.Domain.Exceptions;

namespace Movie.Domain.ValueObjects;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be null or empty");

        email = email.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(email))
            throw new DomainException("Invalid email format");

        if (email.Length > 254) // RFC 5321 limit
            throw new DomainException("Email address is too long");

        return new Email(email);
    }

    public static implicit operator string(Email email) => email.Value;

    public override string ToString() => Value;
}
