namespace Movie.Domain.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
    public string Role { get; set; } = "User"; // Admin/User
}