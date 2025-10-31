using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Application.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// DbContext PostgreSQL (Neon)
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseNpgsql(connectionString));


// Servicios base
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMappers
builder.Services.AddAutoMapper(typeof(UsuarioProfile));

// Controladores
//builder.Services.AddScoped<UsuarioService>();
//builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// CORS Configuration para Angular en localhost:4200
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Importante si usas cookies o autenticación
    });
});

//builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// CORS debe ir después de UseHttpsRedirection y antes de UseAuthorization
app.UseCors("AngularLocalhost");

app.UseAuthorization();

// Expone controllers
app.MapControllers();

app.Run();