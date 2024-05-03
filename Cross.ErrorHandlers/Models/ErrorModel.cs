namespace Cross.ErrorHandlers.Models;

public class ErrorModel
{
    public Guid CorrelationId { get; set; }

    public string Code { get; set; }

    public string? Message { get; set; }

    /// <summary>
    /// Detailed error list
    /// </summary>
    public IDictionary<string, IEnumerable<string>>? Errors { get; set; }

    public ErrorModel() { }

    public ErrorModel(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public ErrorModel(string code, string message, IDictionary<string, IEnumerable<string>> errors)
    {
        Code = code;
        Message = message;
        Errors = errors;
    }
}
