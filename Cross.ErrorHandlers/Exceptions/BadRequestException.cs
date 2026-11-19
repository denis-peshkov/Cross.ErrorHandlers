namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when the request is malformed or contains invalid parameters.
/// </summary>
/// <remarks>
/// This exception is typically thrown when the client sends a request that doesn't meet the expected format or validation rules.
/// <see cref="Middleware.ErrorHandlerMiddleware"/> maps it to HTTP 400 Bad Request with error code <c>BadRequest</c>.
/// An optional sub-code can be supplied and is stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.
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
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message and sub-code.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context. Stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    public BadRequestException(string message, string subCode) : this(message, subCode, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BadRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestException"/> class with a specified error message, sub-code, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context. Stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public BadRequestException(string message, string subCode, Exception? innerException) : base(message, innerException)
    {
        Data.Add("SubCode", subCode);
    }
}
