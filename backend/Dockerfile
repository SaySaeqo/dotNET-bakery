# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory.
WORKDIR /app

# Copy csproj and restore dependencies.
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build the project.
COPY . ./
RUN dotnet publish -c Release -o out ./dotNet_bakery.csproj

# Build runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Define the command to start the application.
ENTRYPOINT ["dotnet", "dotNet_bakery.dll"]