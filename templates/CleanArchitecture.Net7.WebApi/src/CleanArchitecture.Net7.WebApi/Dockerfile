#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/CleanArchitecture.Net7.WebApi/CleanArchitecture.Net7.WebApi.csproj", "src/CleanArchitecture.Net7.WebApi/"]
RUN dotnet restore "src/CleanArchitecture.Net7.WebApi/CleanArchitecture.Net7.WebApi.csproj"
COPY . .
WORKDIR "/src/src/CleanArchitecture.Net7.WebApi"
RUN dotnet build "CleanArchitecture.Net7.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanArchitecture.Net7.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Net7.WebApi.dll"]