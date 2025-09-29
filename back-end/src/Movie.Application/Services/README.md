# Application Services

This folder contains application services that orchestrate complex business workflows and coordinate between multiple domain entities or external services.

## Purpose

Application services handle:
- Complex business workflows that span multiple aggregates
- Coordination with external services
- Cross-cutting concerns at the application level
- Transaction management across multiple operations

## Examples of services that will be added here:

- `IMovieApplicationService` - Complex movie management workflows
- `IUserApplicationService` - User management and authentication workflows  
- `IAuthenticationService` - Authentication and authorization logic
- `INotificationService` - Cross-cutting notification functionality
- `IEmailService` - Email sending capabilities
- `IFileStorageService` - File upload and management

## Guidelines

- Keep business logic in the Domain layer
- Use application services for orchestration only
- Inject domain services and repositories
- Handle transactions at this level
- Coordinate with external services (APIs, file systems, etc.)
