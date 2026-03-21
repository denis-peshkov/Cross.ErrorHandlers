namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when a user is authenticated but not authorized to access a resource.
/// </summary>
/// <remarks>
/// This exception is typically thrown when an authenticated user attempts to access a resource they don't have permission to.
/// It maps to HTTP 403 Forbidden status code.
/// </remarks>
public sealed class ForbiddenException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ForbiddenException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ForbiddenException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
