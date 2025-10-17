using Movie.Application.Common.Interfaces;
using Movie.Application.Common.Models;
using Movie.Application.Features.Movies.DTOs;

namespace Movie.Application.Features.Movies.Queries;

public record GetMoviesQuery : IQuery<PagedResult<MovieListDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}