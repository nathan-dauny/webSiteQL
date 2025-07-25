# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ENV NUGET_PACKAGES=/root/.nuget/packages

WORKDIR /src

# Copier tout le code
COPY . .

# Nettoyer le cache NuGet
RUN dotnet nuget locals all --clear

# Restaurer uniquement le projet web (et pas la solution complète)
RUN dotnet restore webSiteQL/webSiteQL.csproj

# Publier en Release
RUN dotnet publish webSiteQL/webSiteQL.csproj -c Release -o /app/publish

# Étape de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "webSiteQL.dll"]
