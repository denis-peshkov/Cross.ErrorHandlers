namespace Cross.ErrorHandlers.Exceptions;

public sealed class ImageNotFoundException : Exception
{
    public ImageNotFoundException(string message) : base(message)
    {
    }
}
