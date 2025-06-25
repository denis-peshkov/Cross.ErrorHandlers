namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when a requested resource already exists, causing a conflict.
/// </summary>
/// <remarks>
/// This exception is typically thrown when an attempt to create a resource conflicts with an existing one.
/// It maps to HTTP 409 Conflict status code.
/// </remarks>
public sealed class ConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ConflictException(string message) : this(message, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message and sub-code.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific subcode that provides additional error context.</param>
    public ConflictException(string message, string subCode) : this(message, subCode, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ConflictException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message, sub-code, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific subcode that provides additional error context. If provided, it will be stored in the exception's Data dictionary with the key "SubCode".</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ConflictException(string? message, string? subCode, Exception? innerException) : base(message, innerException)
    {
        if (subCode != null)
        {
            Data.Add("SubCode", subCode);
        }
    }
}
