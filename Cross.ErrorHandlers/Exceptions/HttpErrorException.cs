namespace Cross.ErrorHandlers.Exceptions;

public class HttpErrorException : Exception
{
    public HttpErrorException(string message) : base(message)
    {
    }
}
