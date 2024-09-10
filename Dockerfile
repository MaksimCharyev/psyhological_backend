
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BackendPsychSite.API/BackendPsychSite.API.csproj", "BackendPsychSite.API/"]
COPY ["BackendPsychSite.Infrastructure/BackendPsychSite.Infrastructure.csproj", "BackendPsychSite.Infrastructure/"]
COPY ["BackendPsychSite.DataAccess/BackendPsychSite.DataAccess.csproj", "BackendPsychSite.DataAccess/"]
COPY ["BackendPsychSite.UseCases/BackendPsychSite.UseCases.csproj", "BackendPsychSite.UseCases/"]
COPY ["BackendPsychSite.Core/BackendPsychSite.Core.csproj", "BackendPsychSite.Core/"]
RUN dotnet restore "./BackendPsychSite.API/BackendPsychSite.API.csproj"
COPY . .
WORKDIR "/src/BackendPsychSite.API"
RUN dotnet build "./BackendPsychSite.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BackendPsychSite.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendPsychSite.API.dll"]