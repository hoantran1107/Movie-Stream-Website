using System.Linq.Expressions;
using Movie.Domain.Entities;

namespace Movie.Domain.Specifications;

public static class MovieSpecifications
{
    public static Expression<Func<MovieItem, bool>> WithSlug(string slug)
    {
        return movie => movie.Slug.Value == slug;
    }

    public static Expression<Func<MovieItem, bool>> WithTitle(string title)
    {
        return movie => movie.Title.Contains(title);
    }

    public static Expression<Func<MovieItem, bool>> HasStreamingUrl()
    {
        return movie => !string.IsNullOrEmpty(movie.HlsManifestUrl) || !string.IsNullOrEmpty(movie.Mp4Url);
    }

    public static Expression<Func<MovieItem, bool>> CreatedAfter(DateTimeOffset date)
    {
        return movie => movie.CreatedAt > date;
    }

    public static Expression<Func<MovieItem, bool>> DurationBetween(TimeSpan minDuration, TimeSpan maxDuration)
    {
        return movie => movie.Duration.Value >= minDuration && movie.Duration.Value <= maxDuration;
    }
}
