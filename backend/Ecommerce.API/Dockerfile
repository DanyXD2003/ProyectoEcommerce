# Compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el backend (todas las capas)
COPY . .

# Restaurar dependencias del proyecto principal (API)
RUN dotnet restore ./backend/Ecommerce.API/Ecommerce.API.csproj

# Compilar y publicar la API en modo Release
RUN dotnet publish ./backend/Ecommerce.API/Ecommerce.API.csproj -c Release -o /app/out

# Runtime (imagen más ligera para ejecución)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos compilados desde la etapa anterior
COPY --from=build /app/out .

# Exponer el puerto 8080 (Render usa este puerto)
EXPOSE 8080

# Configurar el entorno para escuchar en ese puerto
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Comando de inicio
ENTRYPOINT ["dotnet", "Ecommerce.API.dll"]
