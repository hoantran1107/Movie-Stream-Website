# Movie DTOs

This folder contains Data Transfer Objects for movie-related operations.

## Planned DTOs

### Response DTOs
- **MovieDto** - Complete movie information for API responses
- **MovieListDto** - Lightweight movie info for lists
- **MovieDetailDto** - Extended movie info with additional metadata

### Request DTOs
- **CreateMovieDto** - Input data for creating movies
- **UpdateMovieDto** - Input data for updating movies
- **MovieFilterDto** - Filtering options for movie queries

### Common DTOs
- **PaginatedMoviesDto** - Paginated results wrapper
- **MovieSearchResultDto** - Search results with highlighting

## DTO Guidelines

- DTOs should be simple data containers
- Use AutoMapper for Domain ↔ DTO mapping
- Implement `IMapFrom<T>` for automatic mapping
- Keep DTOs specific to their use case (separate create/update DTOs)

## Example Structure
```csharp
public class MovieDto : IMapFrom<MovieItem>
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    // ... other properties
}
```
