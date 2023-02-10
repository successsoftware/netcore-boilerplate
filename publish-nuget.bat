@echo off
echo Starting publish
dotnet nuget push "artifacts\SSS.CleanArchitecture.Net6.WebApi.1.2.0.nupkg" --api-key oy2a65arnyyq7ybsvtaadpangdz7rwnudflpawz5fpcioi --source https://api.nuget.org/v3/index.json
pause