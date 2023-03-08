@echo off
echo Starting publish
dotnet nuget push "artifacts\SSS.CleanArchitecture.Net7.WebApi.1.0.2.nupkg" --api-key oy2ehhuus4hlfeectp7qbmdhawmzsbsepi7qgyzy4j6jfi --source https://api.nuget.org/v3/index.json
pause