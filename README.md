# ProyectoEcommerce

# ASEGURARSE DE TENER DESOCUPADO EL LOCALHOST:5000
## Backend
cd backend
dotnet restore
dotnet build

### Puerto estándar para todos
dotnet run --launch-profile "http"

### Si el puerto está ocupado
dotnet run --launch-profile "CustomPort"

## Frontend
cd frontend/front-api
ng serve -o
