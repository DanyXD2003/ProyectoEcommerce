# 🧩 Etapa 1: Compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todos los archivos desde la raíz del proyecto actual (Render ya está en backend/Ecommerce.API)
COPY . .

# Restaurar dependencias del proyecto principal
RUN dotnet restore Ecommerce.API.csproj

# Compilar y publicar
RUN dotnet publish Ecommerce.API.csproj -c Release -o /app/out

# 🧩 Etapa 2: Runtime (más ligera)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Configuración de entorno
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Comando de inicio
ENTRYPOINT ["dotnet", "Ecommerce.API.dll"]
