namespace Movie.Domain.Events;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}
