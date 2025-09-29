# Watch History Queries

This folder contains queries (read operations) for watch history functionality.

## Planned Queries

### Get User Watch History
- **Query**: `GetUserWatchHistoryQuery`
- **Handler**: `GetUserWatchHistoryQueryHandler`
- **Purpose**: Get all watch history for a user

### Get Movie Watch Progress
- **Query**: `GetMovieWatchProgressQuery`
- **Handler**: `GetMovieWatchProgressQueryHandler`
- **Purpose**: Get watch progress for specific user + movie

### Get Recently Watched Movies
- **Query**: `GetRecentlyWatchedMoviesQuery`
- **Handler**: `GetRecentlyWatchedMoviesQueryHandler`
- **Purpose**: Get recently watched movies for continue watching

### Get Watch Statistics
- **Query**: `GetWatchStatisticsQuery`
- **Handler**: `GetWatchStatisticsQueryHandler`
- **Purpose**: Get user watch statistics (total time, completed movies, etc.)

## Privacy Considerations

- Users can only access their own watch history
- Admin users may access aggregated statistics only
- Watch history should be anonymizable for analytics
