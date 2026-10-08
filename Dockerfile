# Build stage - uses the full Software Development Kit (SDK) to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish PCHardwareInventoryManager.Api/PCHardwareInventoryManager.Api.csproj -c Release -o /app/publish

# Runtime stage - smaller image, only contains what's needed to run the already-published app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "PCHardwareInventoryManager.Api.dll"]
