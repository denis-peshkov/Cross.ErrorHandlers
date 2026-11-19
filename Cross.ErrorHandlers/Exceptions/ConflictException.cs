namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when a requested resource already exists, causing a conflict.
/// </summary>
/// <remarks>
/// This exception is typically thrown when an attempt to create a resource conflicts with an existing one.
/// <see cref="Middleware.ErrorHandlerMiddleware"/> maps it to HTTP 409 Conflict with error code <c>Conflict</c>.
/// An optional sub-code can be supplied and is stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.
/// </remarks>
public sealed class ConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ConflictException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message and sub-code.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context. Stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    public ConflictException(string message, string subCode) : this(message, subCode, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ConflictException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message, sub-code, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context. Stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ConflictException(string message, string subCode, Exception? innerException) : base(message, innerException)
    {
        Data.Add("SubCode", subCode);
    }
}
