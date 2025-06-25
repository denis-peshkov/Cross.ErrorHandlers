namespace Cross.ErrorHandlers.Models;

public class ApiEnvelope<T>
{
    /// <summary>
    /// Gets or sets the data payload if the request was successful.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets the error details if an error occurred.
    /// </summary>
    public ErrorModel? Error { get; set; }

    public ApiEnvelope() { }

    public ApiEnvelope(T? data) => Data = data;

    public ApiEnvelope(ErrorModel? error) => Error = error;
}
