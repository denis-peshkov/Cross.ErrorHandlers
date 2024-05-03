dotnet build --configuration release ..\Cross.CQRS.sln
REM nuget.exe pack config.nuspec -Symbols -SymbolPackageFormat snupkg
nuget.exe pack config.nuspec -Symbols
