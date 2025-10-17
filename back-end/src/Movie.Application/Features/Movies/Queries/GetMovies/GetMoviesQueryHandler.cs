using AutoMapper;
using Movie.Application.Abstractions;
using Movie.Application.Common.Interfaces;
using Movie.Application.Common.Models;
using Movie.Application.Features.Movies.DTOs;

namespace Movie.Application.Features.Movies.Queries.GetMovies;

public class GetMoviesQueryHandler : IQueryHandler<GetMoviesQuery, PagedResult<MovieListDto>>
{
    private readonly IMovieReadRepository _repository;
    private readonly IMapper _mapper;

    public GetMoviesQueryHandler(IMovieReadRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<MovieListDto>> Handle(
        GetMoviesQuery request,
        CancellationToken cancellationToken)
    {
        // Repository trả về PagedResult<MovieItem>
        var pagedMovies = await _repository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        // Map từ MovieItem sang MovieListDto
        var movieDtos = _mapper.Map<List<MovieListDto>>(pagedMovies.Items);

        // Return PagedResult<MovieListDto>
        return new PagedResult<MovieListDto>
        {
            Items = movieDtos,
            TotalItems = pagedMovies.TotalItems,
            PageNumber = pagedMovies.PageNumber,
            PageSize = pagedMovies.PageSize,
            TotalPages = pagedMovies.TotalPages
        };
    }
}