namespace Movie.Infrastructure.Exceptions;

public class BulkDeleteException : Exception
{
    public BulkDeleteException(string message) : base(message)
    {
        OverrideMessage = message;
    }

    public BulkDeleteException(string message, params object[] args)
        : this(string.Empty, message, args)
    {
    }

    public BulkDeleteException(string code, string message, params object[] args)
        : this(null, code, message, args)
    {
    }

    public BulkDeleteException(Exception innerException, string message, params object[] args)
        : this(innerException, string.Empty, message, args)
    {
    }

    public BulkDeleteException(Exception innerException, string code, string message, params object[] args)
        : base(string.Format(message, args), innerException)
    {
        Code = code;
    }

    public string OverrideMessage { get; }
    public string Code { get; }
}