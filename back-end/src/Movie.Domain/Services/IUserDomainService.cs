using Movie.Domain.Entities;
using Movie.Domain.ValueObjects;

namespace Movie.Domain.Services;

public interface IUserDomainService
{
    Task<bool> IsEmailUniqueAsync(string email, int? excludeUserId = null);
    Task<AppUser?> AuthenticateAsync(string email, string password);
    bool IsValidPasswordResetToken(string token, AppUser user);
    string GeneratePasswordResetToken(AppUser user);
}
