# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["PRN232.LMS.API/PRN232.LMS.API.csproj", "PRN232.LMS.API/"]
COPY ["PRN232.LMS.Services/PRN232.LMS.Services.csproj", "PRN232.LMS.Services/"]
COPY ["PRN232.LMS.Repositories/PRN232.LMS.Repositories.csproj", "PRN232.LMS.Repositories/"]

# Restore dependencies
RUN dotnet restore "PRN232.LMS.API/PRN232.LMS.API.csproj"

# Copy all source code
COPY . .

# Build the application
RUN dotnet build "PRN232.LMS.API/PRN232.LMS.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "PRN232.LMS.API/PRN232.LMS.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
EXPOSE 8443

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "PRN232.LMS.API.dll"]
