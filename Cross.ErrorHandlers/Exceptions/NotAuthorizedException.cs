namespace Cross.ErrorHandlers.Exceptions;

public sealed class NotAuthorizedException : Exception
{
    public NotAuthorizedException(string message) : base(message)
    {
    }
}
