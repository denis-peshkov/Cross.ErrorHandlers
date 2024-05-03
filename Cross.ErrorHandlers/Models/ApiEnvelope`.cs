namespace Cross.ErrorHandlers.Models;

public class ApiEnvelope<T>
{
    public ApiEnvelope() { }

    public ApiEnvelope(T? data) => Data = data;

    public ApiEnvelope(ErrorModel? error) => Error = error;

    /// <summary>
    /// An array of the type found in type
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Detailed error list
    /// </summary>
    public ErrorModel? Error { get; set; }
}
