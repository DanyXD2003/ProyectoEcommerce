using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodoPagoController : ControllerBase
    {
        private readonly MetodoPagoService _metodoPagoService;
        private readonly JwtService _jwtService;

        public MetodoPagoController(MetodoPagoService metodoPagoService, JwtService jwtService)
        {
            _metodoPagoService = metodoPagoService;
            _jwtService = jwtService;
        }

        // ============================================================
        //                     SECCIÓN USUARIO
        // ============================================================

        // 🔹 Obtener todos los métodos de pago del usuario autenticado
        [HttpGet("obtenerMisMetodos")]
        [Authorize]
        public async Task<IActionResult> ObtenerMisMetodos()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                var metodos = await _metodoPagoService.ObtenerPorUsuarioAsync(usuarioId);
                return Ok(metodos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Agregar nuevo método de pago (tarjeta, etc.)
        [HttpPost("agregarMetodo")]
        [Authorize]
        public async Task<IActionResult> AgregarMetodo([FromBody] CrearMetodoPagoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                var metodo = await _metodoPagoService.AgregarAsync(usuarioId, dto);
                return Ok(metodo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Eliminar uno de sus métodos de pago
        [HttpDelete("eliminarMetodo/{metodoId}")]
        [Authorize]
        public async Task<IActionResult> EliminarMetodo(int metodoId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                await _metodoPagoService.EliminarAsync(usuarioId, metodoId);
                return Ok(new { mensaje = "Método de pago eliminado correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // ============================================================
        //                     SECCIÓN ADMINISTRADOR
        // ============================================================

        // 🔹 Obtener todos los métodos de pago (solo admin)
        [HttpGet("admin/obtenerTodos")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ObtenerTodosAdmin()
        {
            try
            {
                var metodos = await _metodoPagoService.ObtenerTodosAdminAsync();
                return Ok(metodos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Actualizar método de pago (solo admin)
        [HttpPut("admin/actualizar")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarMetodoPagoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var actualizado = await _metodoPagoService.ActualizarAsync(dto);
                return Ok(actualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Eliminar método de pago (solo admin)
        [HttpDelete("admin/eliminar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarAdmin(int id)
        {
            try
            {
                await _metodoPagoService.EliminarAdminAsync(id);
                return Ok(new { mensaje = "Método de pago eliminado correctamente por el administrador." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }
    }
}
