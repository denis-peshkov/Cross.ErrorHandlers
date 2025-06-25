namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur during HTTP client operations.
/// </summary>
/// <remarks>
/// This exception is typically thrown when HTTP requests to external services fail
/// or return unexpected responses. It includes the original error details from the service response.
/// It maps to HTTP 400 Bad Request status code by default.
/// </remarks>
public sealed class HttpClientException : Exception
{
    /// <summary>
    /// Gets the error details from the HTTP response.
    /// </summary>
    public ErrorModel? Error { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public HttpClientException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientException"/> class with a specified error message
    /// and error details from the response.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="error">The error details from the HTTP response.</param>
    public HttpClientException(string message, ErrorModel error) : base(message)
    {
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpClientException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
