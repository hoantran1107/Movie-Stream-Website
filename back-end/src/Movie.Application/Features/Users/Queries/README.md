# User Queries

This folder contains queries (read operations) for user-related functionality.

## Planned Queries

### Get User by ID
- **Query**: `GetUserByIdQuery`
- **Handler**: `GetUserByIdQueryHandler`
- **Purpose**: Retrieve user profile information

### Get User by Email
- **Query**: `GetUserByEmailQuery`
- **Handler**: `GetUserByEmailQueryHandler`
- **Purpose**: Find user by email address

### Get Users List
- **Query**: `GetUsersQuery`
- **Handler**: `GetUsersQueryHandler`
- **Purpose**: Get paginated list of users (Admin only)

### Get Current User
- **Query**: `GetCurrentUserQuery`
- **Handler**: `GetCurrentUserQueryHandler`
- **Purpose**: Get current authenticated user details

## Security Notes

- Queries should respect user privacy
- Admin-only queries should be properly secured
- Never expose password hashes or salts
- Filter results based on user permissions
