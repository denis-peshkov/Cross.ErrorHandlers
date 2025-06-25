namespace Cross.ErrorHandlers.Exceptions;

public sealed class HttpErrorException : Exception
{
    public HttpErrorException(string message) : base(message)
    {
    }
}
