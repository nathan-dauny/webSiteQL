# Étape 1 : Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ENV NUGET_PACKAGES=/root/.nuget/packages

WORKDIR /src

# Copier la solution et les projets
COPY webSiteQL.sln ./
COPY webSiteQL/*.csproj webSiteQL/

# Nettoyage cache NuGet
RUN dotnet nuget locals all --clear

# Copier tout le code
COPY . .

# Publier en Release
RUN dotnet publish webSiteQL/webSiteQL.csproj -c Release -o /app/publish

# Étape 2 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "webSiteQL.dll"]
