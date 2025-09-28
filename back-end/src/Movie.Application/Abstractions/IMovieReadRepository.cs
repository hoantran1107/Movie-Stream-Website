using Movie.Domain.Entities;

namespace Movie.Application.Abstractions;

public interface IMovieReadRepository
{
    Task<MovieItem?> GetBySlugAsync(string slug, CancellationToken ct);
    Task<IReadOnlyList<MovieItem>> ListPagedAsync(int page, int pageSize, CancellationToken ct);
}