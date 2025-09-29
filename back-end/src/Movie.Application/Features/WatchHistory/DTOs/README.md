# Watch History DTOs

This folder contains Data Transfer Objects for watch history operations.

## Planned DTOs

### Response DTOs
- **WatchHistoryDto** - Complete watch history information
- **WatchProgressDto** - Watch progress for a specific movie
- **RecentlyWatchedDto** - Recently watched movies with progress
- **WatchStatisticsDto** - User watch statistics

### Request DTOs
- **UpdateWatchProgressDto** - Input for updating watch progress
- **WatchHistoryFilterDto** - Filtering options for watch history

### Computed DTOs
- **ContinueWatchingDto** - Movies to continue watching
- **CompletedMoviesDto** - Completed movies list

## Example Structure
```csharp
public class WatchProgressDto : IMapFrom<WatchHistory>
{
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public TimeSpan LastPosition { get; set; }
    public double ProgressPercentage { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
```
