using Movie.Domain.Common;
using Movie.Domain.Events.MovieEvents;
using Movie.Domain.Exceptions;
using Movie.Domain.ValueObjects;

namespace Movie.Domain.Entities;

public class MovieItem : BaseEntity
{
    // Private constructor for EF Core
    private MovieItem() { }

    private MovieItem(string title, string description, Duration duration, string posterUrl = "",
        string hlsManifestUrl = "", string mp4Url = "")
    {
        ValidateTitle(title);
        ValidateDescription(description);
        ValidateUrls(posterUrl, hlsManifestUrl, mp4Url);

        Title = title.Trim();
        Slug = ValueObjects.Slug.FromTitle(title);
        Description = description?.Trim() ?? "";
        Duration = duration ?? throw new DomainException("Duration cannot be null");
        PosterUrl = posterUrl?.Trim() ?? "";
        HlsManifestUrl = hlsManifestUrl?.Trim() ?? "";
        Mp4Url = mp4Url?.Trim() ?? "";

        AddDomainEvent(new MovieCreatedEvent(Id, Title, Slug.Value));
    }

    public Slug Slug { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = "";
    public Duration Duration { get; private set; } = default!;
    public string PosterUrl { get; private set; } = "";
    public string HlsManifestUrl { get; private set; } = "";
    public string Mp4Url { get; private set; } = "";

    public static MovieItem Create(string title, string description, Duration duration,
        string posterUrl = "", string hlsManifestUrl = "", string mp4Url = "")
    {
        return new MovieItem(title, description, duration, posterUrl, hlsManifestUrl, mp4Url);
    }

    public void UpdateDetails(string title, string description, Duration duration)
    {
        ValidateTitle(title);
        ValidateDescription(description);

        var hasChanges = Title != title.Trim() || Description != description?.Trim() ||
                        Duration != duration;

        if (!hasChanges)
            return;

        Title = title.Trim();
        Description = description?.Trim() ?? "";
        Duration = duration ?? throw new DomainException("Duration cannot be null");

        // Update slug only if title changed
        if (Slug.Value != ValueObjects.Slug.FromTitle(title).Value)
        {
            Slug = ValueObjects.Slug.FromTitle(title);
        }

        MarkAsUpdated();
        AddDomainEvent(new MovieUpdatedEvent(Id, Title, Slug.Value));
    }

    public void UpdateMediaUrls(string posterUrl = "", string hlsManifestUrl = "", string mp4Url = "")
    {
        ValidateUrls(posterUrl, hlsManifestUrl, mp4Url);

        var hasChanges = PosterUrl != (posterUrl?.Trim() ?? "") ||
                        HlsManifestUrl != (hlsManifestUrl?.Trim() ?? "") ||
                        Mp4Url != (mp4Url?.Trim() ?? "");

        if (!hasChanges)
            return;

        PosterUrl = posterUrl?.Trim() ?? "";
        HlsManifestUrl = hlsManifestUrl?.Trim() ?? "";
        Mp4Url = mp4Url?.Trim() ?? "";

        MarkAsUpdated();
    }

    public bool HasStreamingUrl() => !string.IsNullOrWhiteSpace(HlsManifestUrl) || !string.IsNullOrWhiteSpace(Mp4Url);

    public bool HasPoster() => !string.IsNullOrWhiteSpace(PosterUrl);

    public string GetPreferredStreamingUrl()
    {
        // Prefer HLS for better streaming experience
        if (!string.IsNullOrWhiteSpace(HlsManifestUrl))
            return HlsManifestUrl;

        if (!string.IsNullOrWhiteSpace(Mp4Url))
            return Mp4Url;

        throw new DomainException("No streaming URL available for this movie");
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Movie title cannot be null or empty");

        if (title.Trim().Length < 1)
            throw new DomainException("Movie title cannot be empty");

        if (title.Trim().Length > 200)
            throw new DomainException("Movie title cannot be longer than 200 characters");
    }

    private static void ValidateDescription(string? description)
    {
        if (description?.Length > 2000)
            throw new DomainException("Movie description cannot be longer than 2000 characters");
    }

    private static void ValidateUrls(string? posterUrl, string? hlsManifestUrl, string? mp4Url)
    {
        ValidateUrl(posterUrl, nameof(posterUrl));
        ValidateUrl(hlsManifestUrl, nameof(hlsManifestUrl));
        ValidateUrl(mp4Url, nameof(mp4Url));
    }

    private static void ValidateUrl(string? url, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(url))
            return;

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            throw new DomainException($"Invalid {fieldName}: must be a valid URL");

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            throw new DomainException($"Invalid {fieldName}: must use HTTP or HTTPS protocol");
    }
}