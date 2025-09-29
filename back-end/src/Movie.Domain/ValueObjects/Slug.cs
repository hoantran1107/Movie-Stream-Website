using System.Text.RegularExpressions;
using Movie.Domain.Exceptions;

namespace Movie.Domain.ValueObjects;

public record Slug
{
    private static readonly Regex SlugRegex = new(
        @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
        RegexOptions.Compiled);

    public string Value { get; }

    private Slug(string value)
    {
        Value = value;
    }

    public static Slug Create(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException("Slug cannot be null or empty");

        slug = slug.Trim().ToLowerInvariant();

        if (!SlugRegex.IsMatch(slug))
            throw new DomainException("Slug must contain only lowercase letters, numbers, and hyphens");

        if (slug.Length < 3)
            throw new DomainException("Slug must be at least 3 characters long");

        if (slug.Length > 100)
            throw new DomainException("Slug cannot be longer than 100 characters");

        return new Slug(slug);
    }

    public static Slug FromTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be null or empty");

        var slug = title.ToLowerInvariant()
            .Replace(" ", "-")
            .Replace("'", "")
            .Replace("\"", "");

        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
        slug = Regex.Replace(slug, @"-+", "-");
        slug = slug.Trim('-');

        return Create(slug);
    }

    public static implicit operator string(Slug slug) => slug.Value;

    public override string ToString() => Value;
}
