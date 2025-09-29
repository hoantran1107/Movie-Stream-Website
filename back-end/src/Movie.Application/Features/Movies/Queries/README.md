# Movie Queries

This folder contains queries (read operations) for movie-related functionality.

## Planned Queries

### Get Movie by ID

- **Query**: `GetMovieByIdQuery`
- **Handler**: `GetMovieByIdQueryHandler`
- **Purpose**: Retrieve a single movie by its ID

### Get Movie by Slug

- **Query**: `GetMovieBySlugQuery`
- **Handler**: `GetMovieBySlugQueryHandler`
- **Purpose**: Retrieve a movie by its URL-friendly slug

### Get Movies List

- **Query**: `GetMoviesQuery`
- **Handler**: `GetMoviesQueryHandler`
- **Purpose**: Get paginated list of movies with filtering

### Search Movies

- **Query**: `SearchMoviesQuery`
- **Handler**: `SearchMoviesQueryHandler`
- **Purpose**: Full-text search of movies by title/description

## Query Structure

Each query follows this structure:

```
Queries/
├── GetMovieById/
│   ├── GetMovieByIdQuery.cs
│   └── GetMovieByIdQueryHandler.cs
├── GetMovies/
│   ├── GetMoviesQuery.cs
│   └── GetMoviesQueryHandler.cs
└── ...
```
