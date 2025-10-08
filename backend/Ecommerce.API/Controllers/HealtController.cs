using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly EcommerceDbContext _db;
    public HealthController(EcommerceDbContext db) => _db = db;

    /// <summary>
    /// Verifica la conexión a la base de datos PostgreSQL (Neon).
    /// </summary>
    [HttpGet("db")]
    public async Task<IActionResult> CheckDb()
    {
        var connected = await _db.Database.CanConnectAsync();
        return Ok(new
        {
            status = connected ? "connected" : "disconnected",
            database = "PostgreSQL (Neon)"
        });
    }
}
