namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur during user identity management operations.
/// </summary>
/// <remarks>
/// This exception is typically thrown when operations related to user identity management (such as user creation,
/// role assignment, or password management) fail. It maps to HTTP 500 Internal Server Error status code.
/// </remarks>
public sealed class IdentityUserManagerException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserManagerException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public IdentityUserManagerException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityUserManagerException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public IdentityUserManagerException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
