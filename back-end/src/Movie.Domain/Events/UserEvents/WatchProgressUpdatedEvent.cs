namespace Movie.Domain.Events.UserEvents;

public record WatchProgressUpdatedEvent(
    int UserId,
    int MovieId,
    TimeSpan Position,
    TimeSpan Duration,
    DateTimeOffset OccurredAt = default) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;

    public double ProgressPercentage => Duration.TotalSeconds > 0 ? Position.TotalSeconds / Duration.TotalSeconds * 100 : 0;
}
