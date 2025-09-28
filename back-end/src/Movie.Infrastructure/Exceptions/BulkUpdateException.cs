namespace Movie.Infrastructure.Exceptions;

public class BulkUpdateException : Exception
{
    public BulkUpdateException(string message) : base(message)
    {
        OverrideMessage = message;
    }

    public BulkUpdateException(string message, params object[] args)
        : this(string.Empty, message, args)
    {
    }

    public BulkUpdateException(string code, string message, params object[] args)
        : this(null, code, message, args)
    {
    }

    public BulkUpdateException(Exception innerException, string message, params object[] args)
        : this(innerException, string.Empty, message, args)
    {
    }

    public BulkUpdateException(Exception innerException, string code, string message, params object[] args)
        : base(string.Format(message, args), innerException)
    {
        Code = code;
    }

    public string OverrideMessage { get; }
    public string Code { get; }
}