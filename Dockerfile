# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore first for layer caching
COPY ["HomeMaintenanceScheduler.Api.csproj", "./"]
RUN dotnet restore "HomeMaintenanceScheduler.Api.csproj"

# Copy everything else and publish
COPY . .
RUN dotnet publish "HomeMaintenanceScheduler.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Optional: create folder for database
RUN mkdir -p /app/data

COPY --from=build /app/publish .

# Expose app port
EXPOSE 8080

# ASP.NET config
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "HomeMaintenanceScheduler.Api.dll"]