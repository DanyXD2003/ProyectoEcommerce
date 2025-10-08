using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext PostgreSQL (Neon)
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseNpgsql(connectionString));

// Servicios base
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS dev (opcional)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();        // <- siempre
app.UseSwaggerUI();      // <- siempre

app.UseHttpsRedirection();
app.UseCors("DevCors");

// Expone controllers (Swagger detecta estos endpoints)
app.MapControllers();

app.Run();
