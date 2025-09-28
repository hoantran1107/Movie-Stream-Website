using Microsoft.EntityFrameworkCore;
using Movie.Application.Abstractions;
using Movie.Domain.Entities;
using Movie.Infrastructure.Data;

namespace Movie.Infrastructure.Repositories;

public class MovieReadRepository : IMovieReadRepository
{
    private readonly IGenericRepository<MovieItem> _repository = null!;
    private readonly IUnitOfWork _unitOfWork = null!;
    public MovieReadRepository(IGenericRepository<MovieItem> repository, UnitOfWork<AppDbContext> unitofwork)
    {
        _repository = repository;
        _unitOfWork = unitofwork;
    }
    public Task<MovieItem?> GetBySlugAsync(string slug, CancellationToken ct)
       => _repository.GetAll().AsNoTrackingWithIdentityResolution().Where(x=>x.Slug == slug).FirstOrDefaultAsync(ct);

    public Task<IReadOnlyList<MovieItem>> ListPagedAsync(int page, int pageSize, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}