namespace Cross.ErrorHandlers.Exceptions;

public class HttpClientException : Exception
{
    public ErrorModel? Error { get; set; }

    public HttpClientException(string message) : base(message)
    {
    }

    public HttpClientException(ErrorModel errorModel) : base(errorModel.Message)
    {
        Error = errorModel;
    }
}
