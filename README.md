[![Nuget](https://img.shields.io/nuget/v/Cross.ErrorHandlers.svg)](https://nuget.org/packages/Cross.ErrorHandlers/) [![Documentation](https://img.shields.io/badge/docs-wiki-yellow.svg)](https://github.com/denis-peshkov/Cross.ErrorHandlers/wiki)

# Cross.ErrorHandlers

A .NET library providing unified error handling for ASP.NET Core applications. The package implements middleware that catches exceptions and transforms them into standardized JSON responses with proper HTTP status codes.

## Key Features:
- Global exception handling middleware
- Standardized error response format
- Built-in support for common exception types
- Request correlation tracking
- Configurable stack trace visibility
- Consistent logging

## Purpose
Designed to streamline error handling across microservices and web applications while maintaining proper error tracking and debugging capabilities.

## Install with nuget.org:

https://www.nuget.org/packages/Cross.ErrorHandlers

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package Cross.ErrorHandlers
```

Or via Package Manager Console:

```powershell
Install-Package Cross.ErrorHandlers
```

## Quick Start

1. Register the middleware in your `Program.cs` or `Startup.cs`:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ...

        var app = builder.Build();

        // Add error handler middleware (should be first in the pipeline)
        app.UseMiddleware<ErrorHandlerMiddleware>();

        // Other middleware configurations...

        app.Run();
    }
}
```

2. The middleware will automatically catch exceptions and return standardized responses.

## Error Response Format

All errors are returned in a consistent JSON format:

```json
{
  "code": "errorCode",
  "message": "Error description",
  "correlationId": "00000000-0000-0000-0000-000000000000",
  "errors": {
    "field1": ["Error message 1", "Error message 2"],
    "field2": ["Error message 3"]
  }
}
```

## Supported Exception Types

The middleware handles various exception types with appropriate HTTP status codes:

| Exception Type         | HTTP Status Code | Error Code          |
|------------------------|------------------|---------------------|
| ValidationException    | 406              | InvalidParameters   |
| JsonException          | 406              | InvalidParameters   |
| NotFoundException      | 404              | NotFound            |
| ConflictException      | 409              | Conflict            |
| BadRequestException    | 400              | BadRequest          |
| NotAuthorizedException | 401              | NotAuthorized       |
| ForbiddenException     | 403              | Forbidden           |
| HttpClientException    | 400              | InvalidClient       |
| Exception (default)    | 500              | InternalServerError |

## Configuration

Configure stack trace visibility in `appsettings.json`:

```json
{
  "ClearStackTraceErrors": true // Set to false to include stack traces in responses
}
```

## Correlation ID Tracking

The middleware automatically handles correlation IDs:

1. Reads from `X-Correlation-Id` header if provided
2. Generates new GUID if not present
3. Includes in error responses for request tracking

## Custom Exception Example

```csharp
public class YourController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(int id)
    {
        var item = _repository.GetById(id);
        if (item == null)
            throw new NotFoundException($"Item with id {id} not found");
        return Ok(item);
    }
}
```

Response when item not found:

```json
{
  "code": "NotFound",
  "message": "Item with id 123 not found",
  "correlationId": "7b2ab0e6-3c42-4488-a8b5-9b6d15b63e1a",
  "errors": {
    "Exception": ["NotFoundException: Item with id 123 not found"]
  }
}
```

## Issues and Pull Request

Contribution is welcomed. If you would like to provide a PR please add some testing.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Roadmap
