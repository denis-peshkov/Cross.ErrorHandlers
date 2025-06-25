namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur when a requested image resource cannot be found.
/// </summary>
/// <remarks>
/// This exception is typically thrown when attempting to access or manipulate an image that does not exist.
/// It is a specialized version of NotFoundException specifically for image resources.
/// </remarks>
public sealed class ImageNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ImageNotFoundException(string message) : base(message)
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
}
