namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when a requested resource cannot be found.
/// </summary>
/// <remarks>
/// This exception is typically thrown when attempting to access or manipulate a resource that does not exist.
/// It maps to HTTP 404 Not Found status code.
/// </remarks>
public sealed class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
