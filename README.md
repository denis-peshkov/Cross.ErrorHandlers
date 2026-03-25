[![License](https://img.shields.io/github/license/denis-peshkov/Cross.ErrorHandlers)](LICENSE)
[![GitHub Release Date](https://img.shields.io/github/release-date/denis-peshkov/Cross.ErrorHandlers?label=released)](https://github.com/denis-peshkov/Cross.ErrorHandlers/releases)
[![NuGetVersion](https://img.shields.io/nuget/v/Cross.ErrorHandlers.svg)](https://nuget.org/packages/Cross.ErrorHandlers/)
[![NugetDownloads](https://img.shields.io/nuget/dt/Cross.ErrorHandlers.svg)](https://nuget.org/packages/Cross.ErrorHandlers/)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Cross.ErrorHandlers&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Cross.ErrorHandlers)
[![issues](https://img.shields.io/github/issues/denis-peshkov/Cross.ErrorHandlers)](https://github.com/denis-peshkov/Cross.ErrorHandlers/issues)
[![.NET PR](https://github.com/denis-peshkov/Cross.ErrorHandlers/actions/workflows/dotnet.yml/badge.svg?event=pull_request)](https://github.com/denis-peshkov/Cross.ErrorHandlers/actions/workflows/dotnet.yml)

![Size](https://img.shields.io/github/repo-size/denis-peshkov/Cross.ErrorHandlers)
[![GitHub contributors](https://img.shields.io/github/contributors/denis-peshkov/Cross.ErrorHandlers)](https://github.com/denis-peshkov/Cross.ErrorHandlers/contributors)
[![GitHub commits since latest release (by date)](https://img.shields.io/github/commits-since/denis-peshkov/Cross.ErrorHandlers/latest?label=new+commits)](https://github.com/denis-peshkov/Cross.ErrorHandlers/commits/master)
![Activity](https://img.shields.io/github/commit-activity/w/denis-peshkov/Cross.ErrorHandlers)
![Activity](https://img.shields.io/github/commit-activity/m/denis-peshkov/Cross.ErrorHandlers)
![Activity](https://img.shields.io/github/commit-activity/y/denis-peshkov/Cross.ErrorHandlers)

# Cross.ErrorHandlers

A .NET library providing unified error handling for ASP.NET Core applications. The package implements middleware that catches exceptions and transforms them into standardized JSON responses with proper HTTP status codes.

**Target frameworks:** `net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`

## Key Features

- Global exception handling middleware (`ErrorHandlerMiddleware`)
- Responses wrapped in `ApiEnvelope` with an `error` payload (`ErrorModel`)
- Built-in handling for FluentValidation `ValidationException`, `JsonException`, and library-specific exception types
- Request correlation ID (`X-Correlation-Id` header or generated GUID)
- Configurable clearing of the `errors` dictionary via `ClearStackTraceErrors` (see below)
- Optional `IInternalServerLogger` to customize how caught exceptions are logged

## Purpose

Designed to streamline error handling across microservices and web applications while maintaining proper error tracking and debugging capabilities.

## Install NuGet package

Install the _Cross.ErrorHandlers_ [NuGet package](https://www.nuget.org/packages/Cross.ErrorHandlers/) into your ASP.NET Core project:

```powershell
Install-Package Cross.ErrorHandlers
```
or
```bash
dotnet add package Cross.ErrorHandlers
```

## Quick Start

1. Register the middleware early in the pipeline (typically before other middleware that might throw):

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ...

        var app = builder.Build();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        // Other middleware...

        app.Run();
    }
}
```

2. The middleware will automatically catch exceptions and return standardized responses.

## Error Response Format

Errors are serialized as camelCase JSON. The body is an `ApiEnvelope` object: the failure details live under `error` (`ErrorModel`).

```json
{
  "data": null,
  "error": {
    "correlationId": "00000000-0000-0000-0000-000000000000",
    "code": "errorCode",
    "subCode": null,
    "message": "Error description",
    "errors": {
      "field1": ["Error message 1"],
      "Exception": ["Full exception text when details are retained"]
    }
  }
}
```

`subCode` is set when a `ConflictException` carries a sub-code (via `Data["SubCode"]`).

## Supported Exception Types

| Exception type                 | HTTP status | Error code (`ErrorCodeEnum`) |
|--------------------------------|-------------|------------------------------|
| `ValidationException`          | 406         | `InvalidParameters`          |
| `JsonException`                | 406         | `InvalidParameters`          |
| `InvalidOperationException`    | 400         | `InvalidOperation`           |
| `NotFoundException`            | 400         | `NotFound`                   |
| `ConflictException`            | 400         | `Conflict`                   |
| `BadRequestException`          | 400         | `BadRequest`                 |
| `NotAuthorizedException`       | 401         | `NotAuthorized`              |
| `ForbiddenException`           | 403         | `Forbidden`                  |
| `IdentityUserManagerException` | 500         | `UnauthorizedClient`         |
| `HttpClientException`          | 400         | `InvalidClient`              |
| `ImageNotFoundException`       | 400         | `ImageNotFound`              |
| `Exception` (default)          | 500         | `InternalServerError`        |

`NotFoundException`, `HttpClientException`, and `ImageNotFoundException` are not logged again by the middleware (treated as already expected or logged upstream). All other handled types follow the same logging path as in [Custom exception logging](#custom-exception-logging): `IInternalServerLogger` from `RequestServices` when resolvable, otherwise `ILogger<ErrorHandlerMiddleware>`.

## Configuration

`ClearStackTraceErrors` is read with `IConfiguration.GetValue<bool>("ClearStackTraceErrors")`. When `true`, the middleware replaces `error.errors` with an empty object for every response **except** those with status **406** (`NotAcceptable`), where validation/JSON error details are always kept. When the key is missing, the value defaults to `false`.

Example `appsettings.json` (omit the key or set `false` to keep exception detail entries in `errors` for non-406 responses):

```json
{
  "ClearStackTraceErrors": true // Set to false to include stack traces in responses
}
```

Set to `false` to keep the populated `errors` dictionary (e.g. exception details) on other status codes.

## Correlation ID

1. If the request includes a valid GUID in the `X-Correlation-Id` header, that value is used.
2. Otherwise a new GUID is generated.
3. The value is set on the returned `error.correlationId`.

## Custom exception logging

Register an implementation of `Cross.ErrorHandlers.Logging.IInternalServerLogger` in DI. For each logged exception path, the middleware resolves `IInternalServerLogger` from `HttpContext.RequestServices` when that provider is non-null; otherwise it falls back to `ILogger<ErrorHandlerMiddleware>`.

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

Example response when the item is not found (shape and status as produced by the current middleware):

```json
{
  "data": null,
  "error": {
    "code": "NotFound",
    "subCode": null,
    "message": "Item with id 123 not found",
    "correlationId": "7b2ab0e6-3c42-4488-a8b5-9b6d15b63e1a",
    "errors": {
      "Exception": ["NotFoundException: Item with id 123 not found"]
    }
  }
}
```

HTTP status for this case is **400** unless you change the middleware.

## Issues and Pull Requests

Contributions are welcome. Please include tests with substantive changes.

## Contributing

1. Fork the repository on GitHub (creates a copy under your account).
2. Clone **your fork** locally (`git clone https://github.com/<your-username>/Cross.ErrorHandlers.git`).
3. Create a feature branch from the default branch (`git switch -c feature/amazing-feature`).
4. Commit your changes (`git commit -m 'Add some amazing feature'`).
5. Push the branch to **your fork** (`git push -u origin feature/amazing-feature`).
6. Open a Pull Request from your fork’s feature branch into this repository’s default branch.

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
