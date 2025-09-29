# Watch History Commands

This folder contains commands (write operations) for watch history functionality.

## Planned Commands

### Update Watch Progress
- **Command**: `UpdateWatchProgressCommand`
- **Handler**: `UpdateWatchProgressCommandHandler`
- **Validator**: `UpdateWatchProgressCommandValidator`
- **Purpose**: Update user's watch progress for a movie

### Mark Movie as Completed
- **Command**: `MarkMovieAsCompletedCommand`
- **Handler**: `MarkMovieAsCompletedCommandHandler`
- **Purpose**: Mark a movie as fully watched

### Clear Watch History
- **Command**: `ClearWatchHistoryCommand`
- **Handler**: `ClearWatchHistoryCommandHandler`
- **Purpose**: Remove watch history for a user

### Remove Movie from History
- **Command**: `RemoveMovieFromHistoryCommand`
- **Handler**: `RemoveMovieFromHistoryCommandHandler`
- **Purpose**: Remove specific movie from watch history

## Business Rules

- Only authenticated users can update their own watch history
- Watch progress should be validated (0-100% or position <= movie duration)
- Significant progress changes trigger domain events
- Progress updates should be throttled to avoid spam
