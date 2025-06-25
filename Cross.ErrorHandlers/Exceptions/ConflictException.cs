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
    public ConflictException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class with a specified error message and sub-code.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context.</param>
    public ConflictException(string message, string subCode) : base(message)
    {
        Data.Add("SubCode", subCode);
    }
}
