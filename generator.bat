@echo off
echo dotnet build.
dotnet build
echo package template to nuget.
dotnet pack .\templates\templates.csproj -o .\artifacts\ --no-build
dotnet nuget push "$PWD/nuget/*.nupkg" --api-key VYHepH2s+p/+fdsfdsfsdfdsfsfdsfsdfsdfsfsdfsdfsdfsd --source "https://api.nuget.org/v3/index.json"
pause
