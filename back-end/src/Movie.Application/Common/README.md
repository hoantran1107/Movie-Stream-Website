# Application Common

This folder contains shared components used across the application layer.

## Structure

### Interfaces
- **CQRS Interfaces**: `ICommand`, `IQuery`, `ICommandHandler`, `IQueryHandler`
- **Application Service Interfaces**: Common contracts for application services

### Exceptions  
- **ApplicationException**: Base exception for application layer
- **ValidationException**: Thrown when input validation fails
- **NotFoundException**: Thrown when requested resources don't exist
- **UnauthorizedException**: Thrown for authorization failures

### Behaviors
- **ValidationBehavior**: MediatR pipeline behavior for FluentValidation
- **LoggingBehavior**: Request/response logging
- **TransactionBehavior**: Database transaction management
- **PerformanceBehavior**: Performance monitoring

### Mappings
- **MappingProfile**: Base AutoMapper profile
- **IMapFrom**: Interface for automatic mapping configuration

## Usage Guidelines

1. **CQRS Pattern**: Use `ICommand` for writes, `IQuery` for reads
2. **Validation**: All commands should have FluentValidation validators
3. **Exception Handling**: Use specific exceptions for different scenarios
4. **Mapping**: Implement `IMapFrom<T>` on DTOs for automatic mapping
5. **Behaviors**: Pipeline behaviors handle cross-cutting concerns

## Pipeline Order

MediatR pipeline behaviors execute in this order:
1. LoggingBehavior (request logging)
2. ValidationBehavior (input validation)
3. TransactionBehavior (database transactions)
4. PerformanceBehavior (performance monitoring)
5. Handler execution
6. LoggingBehavior (response logging)
