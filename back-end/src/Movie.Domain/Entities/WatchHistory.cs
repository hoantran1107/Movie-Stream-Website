namespace Movie.Domain.Entities;

public class WatchHistory
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MovieItemId { get; set; }
    public TimeSpan LastPosition { get; set; } = TimeSpan.Zero;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}