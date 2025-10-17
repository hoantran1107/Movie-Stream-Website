using Movie.Application.Common.Models;
using Movie.Domain.Entities;

namespace Movie.Application.Abstractions;

public interface IMovieReadRepository
{
    Task<MovieItem?> GetBySlugAsync(string slug, CancellationToken ct);

    Task<PagedResult<MovieItem>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken ct);
}