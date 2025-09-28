namespace Movie.Infrastructure.Exceptions;

public class BulkInsertException : Exception
{
    public BulkInsertException(string message) : base(message)
    {
        OverrideMessage = message;
    }

    public BulkInsertException(string message, params object[] args)
        : this(string.Empty, message, args)
    {
    }

    public BulkInsertException(string code, string message, params object[] args)
        : this(null, code, message, args)
    {
    }

    public BulkInsertException(Exception innerException, string message, params object[] args)
        : this(innerException, string.Empty, message, args)
    {
    }

    public BulkInsertException(Exception innerException, string code, string message, params object[] args)
        : base(string.Format(message, args), innerException)
    {
        Code = code;
    }

    public string OverrideMessage { get; }
    public string Code { get; }
}