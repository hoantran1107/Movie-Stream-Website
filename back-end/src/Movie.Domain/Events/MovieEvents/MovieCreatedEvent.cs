namespace Movie.Domain.Events.MovieEvents;

public record MovieCreatedEvent(
    int MovieId,
    string Title,
    string Slug,
    DateTimeOffset OccurredAt = default) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
}
