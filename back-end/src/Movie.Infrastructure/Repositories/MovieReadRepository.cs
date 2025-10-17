using Microsoft.EntityFrameworkCore;
using Movie.Application.Abstractions;
using Movie.Application.Common.Models;
using Movie.Application.Features.Movies.DTOs;
using Movie.Domain.Entities;
using Movie.Infrastructure.Data;

namespace Movie.Infrastructure.Repositories;

public class MovieReadRepository(IGenericRepository<MovieItem> repository, UnitOfWork<AppDbContext> unitofwork) : IMovieReadRepository
{
    private readonly IGenericRepository<MovieItem> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitofwork;

    public Task<MovieItem?> GetBySlugAsync(string slug, CancellationToken ct)
       => _repository.GetAll().AsNoTrackingWithIdentityResolution().Where(x => x.Slug == slug).FirstOrDefaultAsync(ct);

    public async Task<PagedResult<MovieItem>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct)
    {
        var query = _repository.GetAll().AsNoTracking();
        var movies = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        var totalItems = await query.CountAsync(ct);
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<MovieItem>
        {
            Items = movies,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }
}