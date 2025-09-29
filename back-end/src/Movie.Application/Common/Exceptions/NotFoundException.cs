namespace Movie.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found
/// </summary>
public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key)
        : base($"{name} with id '{key}' was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}
