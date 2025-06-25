# Release Notes

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
- Added IntegrationTests

## 7.2.0 - 24 Jul 2024
- Remove character escaping from json output as plain text

## 7.1.2 - 21 May 2024
- Make base classes with default constructor

## 7.1.1 - 21 May 2024
- Remove useless NuGet packages

## 7.1.0 - 20 May 2024
- Add subcode for ConflictException

## 7.0.2 - 18 May 2024
- Fix pipeline build/publish nuget

## 7.0.1 - 3 May 2024
- Update packages version

## 7.0.0 - 3 May 2024
- Initial version
