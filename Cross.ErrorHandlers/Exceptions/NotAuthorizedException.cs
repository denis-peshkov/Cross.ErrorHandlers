namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when a user is not authenticated to access a resource.
/// </summary>
/// <remarks>
/// This exception is typically thrown when an unauthenticated user attempts to access a resource that requires authentication.
/// It maps to HTTP 401 Unauthorized status code.
/// </remarks>
public sealed class NotAuthorizedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotAuthorizedException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NotAuthorizedException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotAuthorizedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public NotAuthorizedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
