namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur during HTTP operations.
/// </summary>
/// <remarks>
/// This exception is typically thrown when an HTTP operation fails or returns an unexpected result.
/// It serves as a base exception for HTTP-related errors that don't fit into more specific categories.
/// </remarks>
public sealed class HttpErrorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpErrorException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public HttpErrorException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpErrorException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
