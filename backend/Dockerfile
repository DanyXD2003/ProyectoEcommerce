# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar TODO el backend (todas las capas)
COPY ../ .

# Restaurar dependencias
RUN dotnet restore Ecommerce.API/Ecommerce.API.csproj

# Compilar y publicar
RUN dotnet publish Ecommerce.API/Ecommerce.API.csproj -c Release -o /app/out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Ecommerce.API.dll"]
