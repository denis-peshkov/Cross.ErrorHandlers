namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when the request is malformed or contains invalid parameters.
/// </summary>
/// <remarks>
/// This exception is typically thrown when the client sends a request that doesn't meet the expected format or validation rules.
/// It maps to HTTP 400 Bad Request status code.
/// </remarks>
public sealed class BadRequestException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public BadRequestException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
