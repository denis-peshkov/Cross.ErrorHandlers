namespace Cross.ErrorHandlers.Logging;

/// <summary>
/// Logs exceptions that the error middleware records via <see cref="Microsoft.Extensions.Logging.ILogger"/> (typically errors surfaced as HTTP 4xx/5xx).
/// Register an implementation in DI to replace the default <see cref="ILogger{ErrorHandlerMiddleware}"/> message for these paths.
/// </summary>
public interface IInternalServerLogger
{
    void LogInternalServerError(Exception exception, HttpContext httpContext);
}
