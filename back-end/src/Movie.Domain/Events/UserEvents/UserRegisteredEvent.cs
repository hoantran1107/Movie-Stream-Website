namespace Movie.Domain.Events.UserEvents;

public record UserRegisteredEvent(
    int UserId,
    string Email,
    DateTimeOffset OccurredAt = default) : IDomainEvent
{
    public DateTimeOffset OccurredAt { get; } = OccurredAt == default ? DateTimeOffset.UtcNow : OccurredAt;
}
