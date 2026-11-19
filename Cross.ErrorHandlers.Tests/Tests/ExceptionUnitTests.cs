namespace Cross.ErrorHandlers.Tests.Tests;

[TestFixture]
[Category(TestCategory.UNIT)]
public class ExceptionsTests
{
    [Test]
    [TestCase(typeof(BadRequestException))]
    [TestCase(typeof(ConflictException))]
    [TestCase(typeof(FailedDependency))]
    [TestCase(typeof(ForbiddenException))]
    [TestCase(typeof(HttpClientException))]
    [TestCase(typeof(HttpErrorException))]
    [TestCase(typeof(IdentityException))]
    [TestCase(typeof(ImageNotFoundException))]
    [TestCase(typeof(NotAuthorizedException))]
    [TestCase(typeof(NotFoundException))]
    public void Exception_Constructor_WithMessage_DoesNotSetSubCode(Type exceptionType)
    {
        // Arrange
        var message = "Resource already exists";

        // Act
        var exception = (Exception)Activator.CreateInstance(exceptionType, message)!;

        // Assert
        exception.Message.Should().Be(message);
        exception.Data.Contains("SubCode").Should().BeFalse();
    }

    [Test]
    [TestCase(typeof(BadRequestException))]
    [TestCase(typeof(ConflictException))]
    [TestCase(typeof(FailedDependency))]
    [TestCase(typeof(ForbiddenException))]
    [TestCase(typeof(HttpClientException))]
    [TestCase(typeof(HttpErrorException))]
    [TestCase(typeof(IdentityException))]
    [TestCase(typeof(ImageNotFoundException))]
    [TestCase(typeof(NotAuthorizedException))]
    [TestCase(typeof(NotFoundException))]
    public void Exception_Constructor_WithMessageAndSubCode_SetsMessageAndSubCode(Type exceptionType)
    {
        // Arrange
        var message = "Resource already exists";
        var subCode = "DUPLICATE_EMAIL";

        // Act
        var exception = (Exception)Activator.CreateInstance(exceptionType, message, subCode)!;

        // Assert
        exception.Message.Should().Be(message);
        exception.Data.Contains("SubCode").Should().BeTrue();
        exception.Data["SubCode"].Should().Be(subCode);
    }

    [Test]
    [TestCase(typeof(BadRequestException))]
    [TestCase(typeof(ConflictException))]
    [TestCase(typeof(FailedDependency))]
    [TestCase(typeof(ForbiddenException))]
    [TestCase(typeof(HttpClientException))]
    [TestCase(typeof(HttpErrorException))]
    [TestCase(typeof(IdentityException))]
    [TestCase(typeof(ImageNotFoundException))]
    [TestCase(typeof(NotAuthorizedException))]
    [TestCase(typeof(NotFoundException))]
    public void Exception_Constructor_WithMessageSubCodeAndInnerException_SetsAllProperties(Type exceptionType)
    {
        // Arrange
        var message = "test";
        var subCode = "SUB";
        var innerException = new Exception("Inner exception message");

        // Act
        var exception = (Exception)Activator.CreateInstance(exceptionType, message, subCode, innerException)!;

        // Assert
        exception.Message.Should().Be(message);
        exception.Data["SubCode"].Should().Be(subCode);
        exception.InnerException.Should().Be(innerException);
    }

    [Test]
    public void HttpClientException_Constructor_WithErrorModelSubCode_CopiesSubCodeToData()
    {
        var error = new ErrorModel("InvalidClient", "REMOTE_SUB", "remote message", null);

        var exception = new HttpClientException("client failed", error);

        exception.Error.Should().Be(error);
        exception.Data["SubCode"].Should().Be("REMOTE_SUB");
    }

    [Test]
    public void ValidationException_Constructor_WithValidationErrors_SetsErrors()
    {
        // Arrange
        var validationErrors = new List<ValidationFailure>
        {
            new("email", "Invalid email format"),
            new("password", "Password too short")
        };

        // Act
        var exception = new ValidationException(validationErrors);

        // Assert
        exception.Errors.Should().HaveCount(2);
        exception.Errors.Should().Contain(error => error.PropertyName == "email");
        exception.Errors.Should().Contain(error => error.PropertyName == "password");
    }

    [Test]
    [TestCase(typeof(BadRequestException))]
    [TestCase(typeof(ConflictException))]
    [TestCase(typeof(FailedDependency))]
    [TestCase(typeof(ForbiddenException))]
    [TestCase(typeof(HttpClientException))]
    [TestCase(typeof(HttpErrorException))]
    [TestCase(typeof(IdentityException))]
    [TestCase(typeof(ImageNotFoundException))]
    [TestCase(typeof(NotAuthorizedException))]
    [TestCase(typeof(NotFoundException))]
    public void Exception_Constructor_WithInnerException_SetsAllProperties1(Type exceptionType)
    {
        // Arrange
        var message = "test";
        var innerException = new Exception("Inner exception message");

        // Act
        var exception = (Exception)Activator.CreateInstance(exceptionType, message, innerException)!;

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
        exception.InnerException!.Message.Should().Be(innerException.Message);
    }

    [Test]
    [TestCase(typeof(BadRequestException))]
    [TestCase(typeof(ConflictException))]
    [TestCase(typeof(FailedDependency))]
    [TestCase(typeof(ForbiddenException))]
    [TestCase(typeof(HttpClientException))]
    [TestCase(typeof(HttpErrorException))]
    [TestCase(typeof(IdentityException))]
    [TestCase(typeof(ImageNotFoundException))]
    [TestCase(typeof(NotAuthorizedException))]
    [TestCase(typeof(NotFoundException))]
    public void Exception_Constructor_WithNullMessageAndInnerException_SetsDefaultMessage(Type exceptionType)
    {
        // Arrange
        string? message = null;
        var innerException = new Exception("Inner exception message");
        var resultMessage = $"Exception of type '{exceptionType}' was thrown.";

        // Act
        var exception = (Exception)Activator.CreateInstance(exceptionType, message, innerException)!;

        // Assert
        exception.Message.Should().Be(resultMessage);
        exception.InnerException.Should().Be(innerException);
        exception.InnerException!.Message.Should().Be(innerException.Message);
    }

    [Test]
    [TestCase(typeof(BadRequestException))]
    [TestCase(typeof(ConflictException))]
    [TestCase(typeof(FailedDependency))]
    [TestCase(typeof(ForbiddenException))]
    [TestCase(typeof(HttpClientException))]
    [TestCase(typeof(HttpErrorException))]
    [TestCase(typeof(IdentityException))]
    [TestCase(typeof(ImageNotFoundException))]
    [TestCase(typeof(NotAuthorizedException))]
    [TestCase(typeof(NotFoundException))]
    public void ExceptionSerialization_AllExceptionsAreSerializable(Type exceptionType)
    {
        // Arrange
        var message = "test";
        var exception = (Exception)Activator.CreateInstance(exceptionType, message)!;

        // Act
        var serialized = JsonSerializer.Serialize(exception);

        // Assert
        serialized.Should().NotBeNullOrEmpty();
        serialized.Should().Contain("test");
    }
}
