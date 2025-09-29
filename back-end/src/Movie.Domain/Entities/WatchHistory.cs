using Movie.Domain.Common;
using Movie.Domain.Events.UserEvents;
using Movie.Domain.Exceptions;

namespace Movie.Domain.Entities;

public class WatchHistory : BaseEntity
{
    // Private constructor for EF Core
    private WatchHistory() { }

    private WatchHistory(int userId, int movieItemId, TimeSpan lastPosition)
    {
        ValidateUserId(userId);
        ValidateMovieId(movieItemId);
        ValidatePosition(lastPosition);

        UserId = userId;
        MovieItemId = movieItemId;
        LastPosition = lastPosition;
    }

    public int UserId { get; private set; }
    public int MovieItemId { get; private set; }
    public TimeSpan LastPosition { get; private set; } = TimeSpan.Zero;

    // Navigation properties
    public AppUser User { get; private set; } = default!;
    public MovieItem Movie { get; private set; } = default!;

    public static WatchHistory Create(int userId, int movieItemId, TimeSpan position = default)
    {
        return new WatchHistory(userId, movieItemId, position);
    }

    public void UpdateProgress(TimeSpan newPosition, TimeSpan movieDuration)
    {
        ValidatePosition(newPosition);

        if (newPosition > movieDuration)
            throw new DomainException("Watch position cannot exceed movie duration");

        var hasSignificantChange = Math.Abs((newPosition - LastPosition).TotalSeconds) >= 5; // 5 second threshold

        if (!hasSignificantChange)
            return;

        LastPosition = newPosition;
        MarkAsUpdated();

        AddDomainEvent(new WatchProgressUpdatedEvent(UserId, MovieItemId, newPosition, movieDuration));
    }

    public void MarkAsCompleted(TimeSpan movieDuration)
    {
        UpdateProgress(movieDuration, movieDuration);
    }

    public double GetProgressPercentage(TimeSpan movieDuration)
    {
        if (movieDuration.TotalSeconds <= 0)
            return 0;

        var percentage = LastPosition.TotalSeconds / movieDuration.TotalSeconds * 100;
        return Math.Min(Math.Max(percentage, 0), 100); // Clamp between 0-100
    }

    public bool IsCompleted(TimeSpan movieDuration)
    {
        return GetProgressPercentage(movieDuration) >= 95; // Consider 95%+ as completed
    }

    public bool IsStarted()
    {
        return LastPosition.TotalSeconds > 0;
    }

    public TimeSpan GetRemainingTime(TimeSpan movieDuration)
    {
        var remaining = movieDuration - LastPosition;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new DomainException("User ID must be greater than 0");
    }

    private static void ValidateMovieId(int movieItemId)
    {
        if (movieItemId <= 0)
            throw new DomainException("Movie ID must be greater than 0");
    }

    private static void ValidatePosition(TimeSpan position)
    {
        if (position < TimeSpan.Zero)
            throw new DomainException("Watch position cannot be negative");

        if (position > TimeSpan.FromHours(24))
            throw new DomainException("Watch position cannot exceed 24 hours");
    }
}