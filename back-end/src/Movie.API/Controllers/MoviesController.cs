using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movie.Application.Common.Models;
using Movie.Application.Features.Movies.DTOs;
using Movie.Application.Features.Movies.Queries;

namespace Movie.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MoviesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get paginated list of movies
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 10, max: 100)</param>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MovieListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<MovieListDto>>> GetMovies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageSize > 50) pageSize = 50;
        var query = new GetMoviesQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await mediator.Send(query);
        return Ok(result);
    }

}