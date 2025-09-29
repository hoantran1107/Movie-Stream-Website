# Movie Commands

This folder contains commands (write operations) for movie-related functionality.

## Planned Commands

### Create Movie

- **Command**: `CreateMovieCommand`
- **Handler**: `CreateMovieCommandHandler`
- **Validator**: `CreateMovieCommandValidator`
- **Purpose**: Create a new movie item with validation

### Update Movie

- **Command**: `UpdateMovieCommand`
- **Handler**: `UpdateMovieCommandHandler`  
- **Validator**: `UpdateMovieCommandValidator`
- **Purpose**: Update movie details (title, description, duration)

### Delete Movie

- **Command**: `DeleteMovieCommand`
- **Handler**: `DeleteMovieCommandHandler`
- **Purpose**: Remove a movie from the system

### Update Media URLs

- **Command**: `UpdateMovieMediaUrlsCommand`
- **Handler**: `UpdateMovieMediaUrlsCommandHandler`
- **Validator**: `UpdateMovieMediaUrlsCommandValidator`
- **Purpose**: Update streaming URLs for a movie

## Command Structure

Each command follows this structure:

```
Commands/
├── CreateMovie/
│   ├── CreateMovieCommand.cs
│   ├── CreateMovieCommandHandler.cs
│   └── CreateMovieCommandValidator.cs
├── UpdateMovie/
│   ├── UpdateMovieCommand.cs
│   ├── UpdateMovieCommandHandler.cs
│   └── UpdateMovieCommandValidator.cs
└── ...
```
