namespace Cross.ErrorHandlers.UnitTests.Tests;

[TestFixture]
public class ErrorHandlerMiddlewareTests : TestsBase
{
    private Mock<IHostEnvironment> _mockEnv = null!;
    private Mock<ILogger<ErrorHandlerMiddleware>> _mockLogger = null!;
    private ErrorHandlerMiddleware _middleware = null!;
    private HttpContext _context = null!;

    [SetUp]
    public void Setup()
    {
        _mockEnv = new Mock<IHostEnvironment>();
        _mockLogger = new Mock<ILogger<ErrorHandlerMiddleware>>();

        _context = new DefaultHttpContext();
        _context.Response.Body = new MemoryStream();

        // Setup correlation ID header
        _context.Request.Headers["X-Correlation-Id"] = Guid.NewGuid().ToString();

        _middleware = new ErrorHandlerMiddleware(
            next: (ctx) => Task.CompletedTask,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );
    }

    [Test]
    public async Task InvokeAsync_WhenValidationException_ReturnsNotAcceptable()
    {
        // Arrange
        var validationErrors = new List<ValidationFailure>
        {
            new("field1", "Error 1"),
            new("field2", "Error 2")
        };
        var exception = new ValidationException(validationErrors);

        _middleware = new ErrorHandlerMiddleware(
            next: (ctx) => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.NotAcceptable);
        _context.Response.ContentType.Should().Be("application/json");

        // Verify response content
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        var errorModel = response!.Error;
        errorModel.Should().NotBeNull();
        errorModel!.Code.Should().Be(ErrorCodeEnum.InvalidParameters.ToString());
        errorModel.Message.Should().Be("Validation error from the custom middleware");
        errorModel.Errors.Should().ContainKey("field1");
        errorModel.Errors.Should().ContainKey("field2");
    }

    [Test]
    public async Task InvokeAsync_WhenNotFoundException_ReturnsNotFound()
    {
        // Arrange
        var message = "Resource not found";
        var exception = new NotFoundException(message);

        _middleware = new ErrorHandlerMiddleware(
            next: (ctx) => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        var errorModel = response!.Error;
        errorModel.Should().NotBeNull();
        errorModel!.Code.Should().Be(ErrorCodeEnum.NotFound.ToString());
        errorModel.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenUnauthorizedException_ReturnsUnauthorized()
    {
        // Arrange
        var message = "Unauthorized access";
        var exception = new NotAuthorizedException(message);

        _middleware = new ErrorHandlerMiddleware(
            next: (ctx) => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        var errorModel = response!.Error;
        errorModel.Should().NotBeNull();
        errorModel!.Code.Should().Be(ErrorCodeEnum.NotAuthorized.ToString());
        errorModel.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenGeneralException_ReturnsInternalServerError()
    {
        // Arrange
        var exception = new Exception("Unexpected error");

        _middleware = new ErrorHandlerMiddleware(
            next: (ctx) => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        var errorModel = response!.Error;
        errorModel.Should().NotBeNull();
        errorModel!.Code.Should().Be(ErrorCodeEnum.InternalServerError.ToString());
        errorModel.Message.Should().Be("Internal Server Error from the custom middleware");
    }

    [Test]
    public async Task InvokeAsync_WithoutException_PassesThrough()
    {
        // Arrange
        var nextCalled = false;
        _middleware = new ErrorHandlerMiddleware(
            next: (ctx) => { nextCalled = true; return Task.CompletedTask; },
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        nextCalled.Should().BeTrue();
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Test]
    public async Task InvokeAsync_WithCorrelationId_IncludesInResponse()
    {
        // Arrange
        var correlationId = Guid.NewGuid();
        _context.Request.Headers["X-Correlation-Id"] = correlationId.ToString();
        var exception = new Exception("Test error");

        _middleware = new ErrorHandlerMiddleware(
            next: (ctx) => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        var errorModel = response!.Error;
        errorModel.Should().NotBeNull();
        errorModel!.CorrelationId.Should().Be(correlationId);
    }
}
