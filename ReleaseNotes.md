# Release Notes - Cross.ErrorHandlers
https://github.com/denis-peshkov/Cross.ErrorHandlers/releases

## 7.0.0 - 3 May 2024
- Initial version

## 7.0.1 - 3 May 2024
- Update packages version

## 7.0.2 - 18 May 2024
- Fix pipeline build/publish nuget

## 7.1.0 - 20 May 2024
- Add subcode for ConflictException

## 7.1.1 - 21 May 2024
- Remove useless NuGet packages
- Set Error.Message into base.Message for HttpClientException

## 7.1.2 - 21 May 2024
- Make base classes with default constructor

## 7.2.0 - 24 Jul 2024
- Remove character escaping from json output as plain text

## 7.3.0 - 25 Jun 2025

### New Features
- Added subcode support for `ConflictException` for more granular error classification
- Improved base class structure with default constructors for better extensibility

### Improvements
- Removed character escaping from JSON output for cleaner plain text formatting
- Enhanced error message handling in exceptions
- Added documentation improvements and code examples
- Improved error handling granularity

### Dependencies
- Removed unnecessary NuGet packages to reduce dependencies
- Optimized package management

### Bug Fixes
- Fixed `HttpClientException` message handling by properly setting base message
- Resolved JSON formatting issues

### Code Quality
- Applied code style improvements and refinements
- Enhanced overall codebase maintainability
- Improved class structure for better extensibility

### Tests
- Added UnitTests

## 7.4.0 - 21 Mar 2026

### New features
- Optional `IInternalServerLogger` in DI: middleware calls `LogInternalServerError` on your implementation when registered; otherwise falls back to `ILogger<ErrorHandlerMiddleware>`.

### Target frameworks and dependencies
- Library targets `net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0` (previously `net7.0` / `net8.0` only).
- Replaced `Microsoft.AspNetCore.Mvc.Core` with `Microsoft.AspNetCore.Http.Abstractions` and explicit `Microsoft.Extensions.*` packages; FluentValidation updated to `11.12.0`.
- Added `Microsoft.SourceLink.GitHub` for debugging (symbols).

### NuGet package
- Packaged assemblies for `net6.0`, `net9.0`, `net10.0`; nuspec dependencies and copyright aligned with the project.

### Documentation
- README updated: `ApiEnvelope` / `ErrorModel`, features, configuration (`ClearStackTraceErrors`), correlation ID, exception table, custom logging.

### Tests
- Extended middleware unit tests (correlation ID, `ClearStackTraceErrors`, `IInternalServerLogger`, `HttpClientException` paths, `ConflictException`); added `ModelsTests` for `ApiEnvelope` and `ErrorModel`.

### CI
- GitHub Actions: `setup-dotnet` installs SDK bands `6.0.x`–`10.0.x` to match target frameworks.

## 7.5.0 - 21 Mar 2026
- Fix pipeline
- Fix packages version in nuspec
