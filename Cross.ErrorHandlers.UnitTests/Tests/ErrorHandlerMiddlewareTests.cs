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

    [Test]
    public async Task InvokeAsync_WhenJsonException_ReturnsNotAcceptable()
    {
        // Arrange
        var message = "Invalid JSON format";
        var exception = new JsonException(message);

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

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        var errorModel = response!.Error;
        errorModel.Should().NotBeNull();
        errorModel!.Code.Should().Be(ErrorCodeEnum.InvalidParameters.ToString());
        errorModel.Message.Should().Be("Validation error from the custom middleware");
        errorModel.Errors.Should().NotBeNull().And.ContainKey("Exception");
        errorModel.Errors!["Exception"].Should().Contain(message);
    }

    [Test]
    public async Task InvokeAsync_WhenInvalidOperationException_ReturnsBadRequest()
    {
        // Arrange
        var message = "Invalid operation performed";
        var exception = new InvalidOperationException(message);

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
        errorModel!.Code.Should().Be(ErrorCodeEnum.InvalidOperation.ToString());
        errorModel.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenConflictException_ReturnsBadRequest()
    {
        // Arrange
        var message = "Resource conflict";
        var subCode = "DuplicateEntry";
        var exception = new ConflictException(message, subCode);

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
        errorModel!.Code.Should().Be(ErrorCodeEnum.Conflict.ToString());
        errorModel.Message.Should().Be(message);
        errorModel.SubCode.Should().Be(subCode);
    }

    [Test]
    public async Task InvokeAsync_WhenBadRequestException_ReturnsBadRequest()
    {
        // Arrange
        var message = "Bad request error";
        var exception = new BadRequestException(message);

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
        errorModel!.Code.Should().Be(ErrorCodeEnum.BadRequest.ToString());
        errorModel.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenForbiddenException_ReturnsForbidden()
    {
        // Arrange
        var message = "Access forbidden";
        var exception = new ForbiddenException(message);

        _middleware = new ErrorHandlerMiddleware(
            next: (ctx) => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        // Act
        await _middleware.InvokeAsync(_context);

        // Assert
        _context.Response.StatusCode.Should().Be((int)HttpStatusCode.Forbidden);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        var errorModel = response!.Error;
        errorModel.Should().NotBeNull();
        errorModel!.Code.Should().Be(ErrorCodeEnum.Forbidden.ToString());
        errorModel.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenIdentityUserManagerException_ReturnsInternalServerError()
    {
        // Arrange
        var message = "Identity operation failed";
        var exception = new IdentityUserManagerException(message);

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
        errorModel!.Code.Should().Be(ErrorCodeEnum.UnauthorizedClient.ToString());
        errorModel.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenHttpClientException_ReturnsBadRequest()
    {
        // Arrange
        var message = "HTTP client error";
        var errorDetails = new Dictionary<string, IEnumerable<string>>
        {
            { "field1", new[] { "Error 1" } },
            { "field2", new[] { "Error 2" } }
        };
        var errorModel = new ErrorModel(ErrorCodeEnum.InvalidClient.ToString(), subCode: null, message, errorDetails);
        var exception = new HttpClientException(message, errorModel);

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

        var responseError = response!.Error;
        responseError.Should().NotBeNull();
        responseError!.Code.Should().Be(ErrorCodeEnum.InvalidClient.ToString());
        responseError.Message.Should().Be("Error 1\nError 2");
    }

    [Test]
    public async Task InvokeAsync_WhenImageNotFoundException_ReturnsBadRequest()
    {
        // Arrange
        var message = "Image not found";
        var exception = new ImageNotFoundException(message);

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
        errorModel!.Code.Should().Be(ErrorCodeEnum.ImageNotFound.ToString());
        errorModel.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenCorrelationIdHeaderMissing_GeneratesCorrelationId()
    {
        _context.Request.Headers.Remove("X-Correlation-Id");

        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw new Exception("x"),
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        await _middleware.InvokeAsync(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        response!.Error!.CorrelationId.Should().NotBe(Guid.Empty);
    }

    [Test]
    public async Task InvokeAsync_WhenCorrelationIdHeaderInvalid_GeneratesNewGuid()
    {
        _context.Request.Headers["X-Correlation-Id"] = "not-a-valid-guid";

        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw new Exception("x"),
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        await _middleware.InvokeAsync(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        response!.Error!.CorrelationId.Should().NotBe(Guid.Empty);
    }

    [Test]
    public async Task InvokeAsync_WhenIInternalServerLoggerRegistered_CallsCustomLogger()
    {
        var mockInternalLogger = new Mock<IInternalServerLogger>();
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(x => x.GetService(typeof(IInternalServerLogger))).Returns(mockInternalLogger.Object);
        _context.RequestServices = serviceProvider.Object;

        var exception = new Exception("logged via custom logger");
        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        await _middleware.InvokeAsync(_context);

        mockInternalLogger.Verify(x => x.LogInternalServerError(exception, _context), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_WhenClearStackTraceErrorsTrue_ClearsErrorsForNon406()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["ClearStackTraceErrors"] = "true" })
            .Build();

        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw new Exception("boom"),
            _mockEnv.Object,
            _mockLogger.Object,
            config
        );

        await _middleware.InvokeAsync(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        response!.Error!.Errors.Should().NotBeNull().And.BeEmpty();
    }

    [Test]
    public async Task InvokeAsync_WhenClearStackTraceErrorsTrue_KeepsErrorsFor406()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["ClearStackTraceErrors"] = "true" })
            .Build();

        var validationErrors = new List<ValidationFailure> { new("f", "e") };
        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw new ValidationException(validationErrors),
            _mockEnv.Object,
            _mockLogger.Object,
            config
        );

        await _middleware.InvokeAsync(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        response!.Error!.Errors.Should().NotBeNull().And.ContainKey("f");
    }

    [Test]
    public async Task InvokeAsync_WhenHttpClientException_MessageOnly_UsesMessage()
    {
        const string message = "plain client error";
        var exception = new HttpClientException(message);

        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        await _middleware.InvokeAsync(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        response!.Error!.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenHttpClientException_ErrorWithNullErrors_UsesExceptionMessage()
    {
        const string message = "outer";
        var errorModel = new ErrorModel(ErrorCodeEnum.InvalidClient.ToString(), subCode: null, message, errors: null);
        var exception = new HttpClientException("outer", errorModel);

        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        await _middleware.InvokeAsync(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        response!.Error!.Message.Should().Be(message);
    }

    [Test]
    public async Task InvokeAsync_WhenConflictException_WithoutSubCode_SubCodeIsNull()
    {
        var exception = new ConflictException("conflict without sub");

        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        await _middleware.InvokeAsync(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        response!.Error!.SubCode.Should().BeNull();
        response.Error!.Code.Should().Be(ErrorCodeEnum.Conflict.ToString());
    }

    [Test]
    public async Task InvokeAsync_WhenConflictException_SubCodeKeyPresentWithNullValue_SubCodeNull()
    {
        var exception = new ConflictException("conflict");
        exception.Data["SubCode"] = null!;

        _middleware = new ErrorHandlerMiddleware(
            next: _ => throw exception,
            _mockEnv.Object,
            _mockLogger.Object,
            Configuration
        );

        await _middleware.InvokeAsync(_context);

        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var response = await JsonSerializer.DeserializeAsync<ApiEnvelope<object>>(
            _context.Response.Body,
            ErrorHandlerMiddleware.JsonCamelCaseSerializerOptions
        );

        response!.Error!.SubCode.Should().BeNull();
    }
}
