#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AspTemplate.API/AspTemplate.API.csproj", "AspTemplate.API/"]
COPY ["AspTemplate.Application/AspTemplate.Application.csproj", "AspTemplate.Application/"]
COPY ["AspTemplate.Core/AspTemplate.Core.csproj", "AspTemplate.Core/"]
COPY ["AspTemplate.Persistence/AspTemplate.Persistence.csproj", "AspTemplate.Persistence/"]
COPY ["AspTemplate.Infrastructure/AspTemplate.Infrastructure.csproj", "AspTemplate.Infrastructure/"]
RUN dotnet restore "./AspTemplate.API/AspTemplate.API.csproj"
COPY . .
WORKDIR "/src/AspTemplate.API"
RUN dotnet build "./AspTemplate.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AspTemplate.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspTemplate.API.dll"]