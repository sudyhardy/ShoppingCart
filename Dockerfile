# Use the official .NET SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy the .csproj and restore any dependencies (via nuget)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Publish the app to a folder in the container
RUN dotnet publish -c Release -o /app/publish

# Define the base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the app from the build image
COPY --from=build /app/publish .

# Define the entry point
ENTRYPOINT ["dotnet", "ShoppingCart.dll"]