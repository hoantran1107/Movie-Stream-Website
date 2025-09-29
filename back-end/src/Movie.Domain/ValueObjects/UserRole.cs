using Movie.Domain.Exceptions;

namespace Movie.Domain.ValueObjects;

public record UserRole
{
    public static readonly UserRole Admin = new("Admin");
    public static readonly UserRole User = new("User");
    public static readonly UserRole Moderator = new("Moderator");

    private static readonly HashSet<string> ValidRoles = new()
    {
        "Admin", "User", "Moderator"
    };

    public string Value { get; }

    private UserRole(string value)
    {
        Value = value;
    }

    public static UserRole Create(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new DomainException("Role cannot be null or empty");

        role = role.Trim();

        if (!ValidRoles.Contains(role))
            throw new DomainException($"Invalid role: {role}. Valid roles are: {string.Join(", ", ValidRoles)}");

        return new UserRole(role);
    }

    public bool IsAdmin() => Value == "Admin";
    public bool IsModerator() => Value == "Moderator";
    public bool IsUser() => Value == "User";

    public bool CanManageMovies() => IsAdmin() || IsModerator();
    public bool CanManageUsers() => IsAdmin();

    public static implicit operator string(UserRole role) => role.Value;

    public override string ToString() => Value;
}
