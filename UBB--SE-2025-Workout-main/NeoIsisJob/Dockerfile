# Base image for .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Set working directory for build context
WORKDIR /src

# Copy only csproj files first to leverage Docker cache
COPY ["Workout.Web/Workout.Web.csproj", "Workout.Web/"]
COPY ["Workout.Core/Workout.Core.csproj", "Workout.Core/"]

# Restore NuGet packages
RUN dotnet restore "Workout.Web/Workout.Web.csproj"

# Copy the full source code
COPY . .

# Set working directory to the web project folder
WORKDIR "/src/Workout.Web"

# Build the application
RUN dotnet build "Workout.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Workout.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the app
ENTRYPOINT ["dotnet", "Workout.Web.dll"]
