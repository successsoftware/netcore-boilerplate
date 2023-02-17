@echo off
echo dotnet build.
dotnet build
echo package template to nuget.
dotnet pack .\templates\templates.csproj -o .\artifacts\ --no-build
pause
