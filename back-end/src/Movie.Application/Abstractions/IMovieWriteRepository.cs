using Movie.Domain.Entities;

namespace Movie.Application.Abstractions;

public interface IMovieWriteRepository
{
    Task AddAsync(MovieItem entity, CancellationToken ct);
    void Update(MovieItem entity);
    void Remove(MovieItem entity);
}