namespace Cross.ErrorHandlers.Models;

/// <summary>
/// Represents the standardized error model returned in API responses.
/// </summary>
public class ErrorModel
{
    /// <summary>
    /// Gets or sets the correlation ID for request tracking.
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the error code identifying the type of error.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the error subcode that specifies the type of error more detailed.
    /// </summary>
    public string? SubCode { get; set; }

    /// <summary>
    /// Gets or sets the human-readable error message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of validation errors, where the key is the field name
    /// and the value is an array of error messages.
    /// </summary>
    public IDictionary<string, IEnumerable<string>>? Errors { get; set; }

    public ErrorModel() {}

    public ErrorModel(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public ErrorModel(string code, string message, IDictionary<string, IEnumerable<string>>? errors)
    {
        Code = code;
        Message = message;
        Errors = errors;
    }

    public ErrorModel(string code, string? subCode, string message, IDictionary<string, IEnumerable<string>>? errors)
    {
        Code = code;
        SubCode = subCode;
        Message = message;
        Errors = errors;
    }
}
