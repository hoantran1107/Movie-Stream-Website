# User DTOs

This folder contains Data Transfer Objects for user-related operations.

## Planned DTOs

### Response DTOs
- **UserDto** - Public user information (no sensitive data)
- **UserProfileDto** - Extended user profile information
- **AuthUserDto** - User info after successful authentication

### Request DTOs
- **RegisterUserDto** - User registration input
- **LoginUserDto** - Login credentials
- **ChangePasswordDto** - Password change input
- **UpdateUserRoleDto** - Role update input

### Authentication DTOs
- **AuthTokenDto** - JWT token response
- **RefreshTokenDto** - Token refresh request

## Security Guidelines

- Never include password hashes or salts in DTOs
- Use separate DTOs for public vs private user data
- Validate all input DTOs
- Include only necessary fields in response DTOs

## Example Structure
```csharp
public class UserDto : IMapFrom<AppUser>
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsEmailVerified { get; set; }
    // No password data!
}
```
