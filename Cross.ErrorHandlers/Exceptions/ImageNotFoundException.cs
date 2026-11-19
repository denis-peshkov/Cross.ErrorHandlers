namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when a requested image resource cannot be found.
/// </summary>
/// <remarks>
/// This exception is typically thrown when attempting to access or manipulate an image that does not exist.
/// Unlike <see cref="NotFoundException"/>, it uses a dedicated error code for image resources.
/// <see cref="Middleware.ErrorHandlerMiddleware"/> maps it to HTTP 400 Bad Request with error code <c>ImageNotFound</c>.
/// An optional sub-code can be supplied and is stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.
/// </remarks>
public sealed class ImageNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ImageNotFoundException(string? message) : this(message, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageNotFoundException"/> class with a specified error message and sub-code.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context. When not null, it is stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    public ImageNotFoundException(string? message, string? subCode) : this(message, subCode, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ImageNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageNotFoundException"/> class with a specified error message, sub-code, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context. When not null, it is stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ImageNotFoundException(string? message, string? subCode, Exception? innerException) : base(message, innerException)
    {
        if (subCode != null)
        {
            Data.Add("SubCode", subCode);
        }
    }
}
