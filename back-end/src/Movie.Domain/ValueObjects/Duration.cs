using Movie.Domain.Exceptions;

namespace Movie.Domain.ValueObjects;

public record Duration
{
    public TimeSpan Value { get; }

    private Duration(TimeSpan value)
    {
        Value = value;
    }

    public static Duration Create(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
            throw new DomainException("Duration must be greater than zero");

        if (duration > TimeSpan.FromHours(12))
            throw new DomainException("Duration cannot be longer than 12 hours");

        return new Duration(duration);
    }

    public static Duration FromMinutes(int minutes)
    {
        if (minutes <= 0)
            throw new DomainException("Duration must be greater than zero minutes");

        return Create(TimeSpan.FromMinutes(minutes));
    }

    public static Duration FromSeconds(int seconds)
    {
        if (seconds <= 0)
            throw new DomainException("Duration must be greater than zero seconds");

        return Create(TimeSpan.FromSeconds(seconds));
    }

    public int TotalMinutes => (int)Value.TotalMinutes;
    public int TotalSeconds => (int)Value.TotalSeconds;

    public string ToDisplayString()
    {
        if (Value.TotalHours >= 1)
            return $"{(int)Value.TotalHours}h {Value.Minutes}m";
        
        return $"{Value.Minutes}m {Value.Seconds}s";
    }

    public static implicit operator TimeSpan(Duration duration) => duration.Value;
    
    public override string ToString() => ToDisplayString();
}
