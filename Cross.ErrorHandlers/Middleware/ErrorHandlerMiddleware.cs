namespace Cross.ErrorHandlers.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IHostEnvironment _env;

    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    private readonly IConfiguration _configuration;

    public static JsonSerializerOptions JsonCamelCaseSerializerOptions { get; } = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        IHostEnvironment env,
        ILogger<ErrorHandlerMiddleware> logger,
        IConfiguration configuration)
    {
        _next = next;
        _env = env;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            LogInternalServerError(ex, httpContext);

            var errors = ex.Errors
                .GroupBy(o => o.PropertyName)
                .ToDictionary(group => group.Key, group => group.Select(o => o.ErrorMessage));

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.InvalidParameters.ToString(),
                    message: "Validation error from the custom middleware",
                    errors: errors
                ),
                HttpStatusCode.NotAcceptable);
        }
        catch (JsonException ex)
        {
            LogInternalServerError(ex, httpContext);

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.InvalidParameters.ToString(),
                    message: "Validation error from the custom middleware",
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.Message } } }
                ),
                HttpStatusCode.NotAcceptable);
        }
        catch (InvalidOperationException ex)
        {
            LogInternalServerError(ex, httpContext);

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.InvalidOperation.ToString(),
                    message: ex.Message,
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ));
        }
        catch (NotFoundException ex)
        {
            // do not log message

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.NotFound.ToString(),
                    message: ex.Message,
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ));
        }
        catch (ConflictException ex)
        {
            LogInternalServerError(ex, httpContext);

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.Conflict.ToString(),
                    subCode: ex.Data["SubCode"]?.ToString(),
                    message: ex.Message,
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ));
        }
        catch (BadRequestException ex)
        {
            LogInternalServerError(ex, httpContext);

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.BadRequest.ToString(),
                    message: ex.Message,
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ));
        }
        catch (NotAuthorizedException ex)
        {
            LogInternalServerError(ex, httpContext);

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.NotAuthorized.ToString(),
                    message: ex.Message,
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ),
                HttpStatusCode.Unauthorized);
        }
        catch (ForbiddenException ex)
        {
            LogInternalServerError(ex, httpContext);

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.Forbidden.ToString(),
                    message: ex.Message,
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ),
                HttpStatusCode.Forbidden);
        }
        catch (IdentityUserManagerException ex)
        {
            LogInternalServerError(ex, httpContext);

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.UnauthorizedClient.ToString(),
                    message: ex.Message,
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ),
                HttpStatusCode.InternalServerError);
        }
        catch (HttpClientException ex)
        {
            // do not log message again, already logged into some microservice

            // TODO: fix after Armada leave
            var message = ex.Errors != null ? string.Join("\n", ex.Errors.Values.SelectMany(v => v)) : ex.Message;

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.InvalidClient.ToString(),
                    message: message,
                    errors: null
                ));
        }
        catch (ImageNotFoundException ex)
        {
            // do not log the message

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.ImageNotFound.ToString(),
                    message: ex.Message,
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ));
        }
        catch (Exception ex)
        {
            LogInternalServerError(ex, httpContext);

            await HandleExceptionAsync(
                httpContext,
                new ErrorModel
                (
                    code: ErrorCodeEnum.InternalServerError.ToString(),
                    message: "Internal Server Error from the custom middleware",
                    errors: new Dictionary<string, IEnumerable<string>>() { { "Exception", new[] { ex.ToString() } } }
                ),
                HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, ErrorModel errorModel, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        errorModel.CorrelationId = GetCorrelationId(context);

        var clearStackTraceErrors = _configuration.GetValue<bool>("ClearStackTraceErrors");

        if (clearStackTraceErrors && statusCode != HttpStatusCode.NotAcceptable)
        {
            errorModel.Errors = new Dictionary<string, IEnumerable<string>>();
        }

        await JsonSerializer.SerializeAsync(context.Response.Body, new ApiEnvelope<object>(errorModel), JsonCamelCaseSerializerOptions);
    }

    private void LogInternalServerError(Exception ex, HttpContext context)
        => _logger.LogError(ex, "{CorrelationId} {ExceptionType}: {Message}", GetCorrelationId(context), ex.GetType(), ex.Message);

    private static Guid GetCorrelationId(HttpContext context)
    {
        var headers = context.Request.Headers
            .Select(x => x)
            .ToDictionary(x => x.Key.ToLower(), x => x.Value);

        var correlationId = GetFirstHeaderValueOrDefault<string>(headers, "X-Correlation-Id");

        if (!Guid.TryParse(correlationId, out var result))
        {
            result = Guid.NewGuid();
        }

        return result;
    }

    private static T? GetFirstHeaderValueOrDefault<T>(Dictionary<string, StringValues> headers, string headerKey)
    {
        var toReturn = default(T);

        if (!headers.TryGetValue(headerKey.ToLower(), out var headerValues))
        {
            return toReturn;
        }

        var valueString = headerValues.FirstOrDefault();
        if (valueString != null)
        {
            toReturn = (T)Convert.ChangeType(valueString, typeof(T));
        }

        return toReturn;
    }
}
