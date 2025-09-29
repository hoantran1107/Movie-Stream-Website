using Movie.Domain.Entities;

namespace Movie.Domain.Services;

public interface IMovieDomainService
{
    Task<bool> IsSlugUniqueAsync(string slug, int? excludeMovieId = null);
    Task<bool> CanUserAccessMovieAsync(int userId, int movieId);
    Task<WatchHistory?> GetOrCreateWatchHistoryAsync(int userId, int movieId);
}
