using System.Security.Cryptography;
using System.Text;
using Movie.Domain.Common;
using Movie.Domain.Events.UserEvents;
using Movie.Domain.Exceptions;
using Movie.Domain.ValueObjects;

namespace Movie.Domain.Entities;

public class AppUser : BaseEntity
{
    // Private constructor for EF Core
    private AppUser() { }

    private AppUser(Email email, string password, UserRole? role = null)
    {
        Email = email ?? throw new DomainException("Email cannot be null");
        Role = role ?? UserRole.User;
        SetPassword(password);

        AddDomainEvent(new UserRegisteredEvent(Id, Email.Value));
    }

    public Email Email { get; private set; } = default!;
    public byte[] PasswordHash { get; private set; } = default!;
    public byte[] PasswordSalt { get; private set; } = default!;
    public UserRole Role { get; private set; } = UserRole.User;
    public DateTimeOffset? LastLoginAt { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTimeOffset? EmailVerifiedAt { get; private set; }

    public static AppUser Create(string email, string password, string? role = null)
    {
        var emailVo = Email.Create(email);
        var roleVo = role != null ? UserRole.Create(role) : UserRole.User;

        return new AppUser(emailVo, password, roleVo);
    }

    public void ChangePassword(string currentPassword, string newPassword)
    {
        if (!VerifyPassword(currentPassword))
            throw new DomainException("Current password is incorrect");

        SetPassword(newPassword);
        MarkAsUpdated();
    }

    public void UpdateRole(UserRole newRole)
    {
        if (Role == newRole)
            return;

        Role = newRole ?? throw new DomainException("Role cannot be null");
        MarkAsUpdated();
    }

    public void VerifyEmail()
    {
        if (IsEmailVerified)
            return;

        IsEmailVerified = true;
        EmailVerifiedAt = DateTimeOffset.UtcNow;
        MarkAsUpdated();
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTimeOffset.UtcNow;
        MarkAsUpdated();
    }

    public bool VerifyPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        return VerifyPasswordHash(password, PasswordHash, PasswordSalt);
    }

    public bool CanManageMovies() => Role.CanManageMovies();

    public bool CanManageUsers() => Role.CanManageUsers();

    public bool IsActive() => IsEmailVerified;

    private void SetPassword(string password)
    {
        ValidatePassword(password);
        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new DomainException("Password cannot be null or empty");

        if (password.Length < 8)
            throw new DomainException("Password must be at least 8 characters long");

        if (password.Length > 128)
            throw new DomainException("Password cannot be longer than 128 characters");

        // Check for at least one uppercase, one lowercase, and one digit
        var hasUpper = password.Any(char.IsUpper);
        var hasLower = password.Any(char.IsLower);
        var hasDigit = password.Any(char.IsDigit);

        if (!hasUpper || !hasLower || !hasDigit)
            throw new DomainException("Password must contain at least one uppercase letter, one lowercase letter, and one digit");
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}