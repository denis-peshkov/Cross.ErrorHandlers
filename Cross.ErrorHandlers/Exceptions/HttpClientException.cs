namespace Cross.ErrorHandlers.Exceptions;

/// <summary>
/// Represents errors that occur during HTTP client operations.
/// </summary>
/// <remarks>
/// This exception is typically thrown when HTTP requests to external services fail
/// or return unexpected responses. It can include the original error details from the service response via <see cref="Error"/>.
/// <see cref="Middleware.ErrorHandlerMiddleware"/> maps it to HTTP 400 Bad Request with error code <c>InvalidClient</c>.
/// An optional sub-code can be supplied (or copied from <see cref="ErrorModel.SubCode"/>) and is stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.
/// </remarks>
public sealed class HttpClientException : Exception
{
    /// <summary>
    /// Gets the error details from the HTTP response, if provided.
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
    /// Initializes a new instance of the <see cref="HttpClientException"/> class with a specified error message and sub-code.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context. Stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    public HttpClientException(string message, string subCode) : this(message, subCode, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientException"/> class with a specified error message
    /// and error details from the response.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="error">The error details from the HTTP response. When <see cref="ErrorModel.SubCode"/> is not null, it is also stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    public HttpClientException(string message, ErrorModel error) : base(message)
    {
        Error = error;
        if (error.SubCode != null)
        {
            Data.Add("SubCode", error.SubCode);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpClientException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientException"/> class with a specified error message, sub-code, and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="subCode">A specific sub-code that provides additional error context. Stored in <see cref="Exception.Data"/> under the key <c>SubCode</c>.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpClientException(string message, string subCode, Exception? innerException) : base(message, innerException)
    {
        Data.Add("SubCode", subCode);
    }
}
