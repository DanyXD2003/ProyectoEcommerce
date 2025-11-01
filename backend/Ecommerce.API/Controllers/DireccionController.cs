using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Services;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DireccionController : ControllerBase
    {
        private readonly DireccionService _direccionService;
        private readonly JwtService _jwtService;

        public DireccionController(DireccionService direccionService, JwtService jwtService)
        {
            _direccionService = direccionService;
            _jwtService = jwtService;
        }

        [HttpPost("crearDireccion")]
        [Authorize]
        public async Task<IActionResult> Crear([FromBody] CrearDireccionDto dto)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var token = authHeader.StartsWith("Bearer ")
                    ? authHeader.Substring("Bearer ".Length).Trim()
                    : authHeader.Trim();
                Console.WriteLine(token);
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                var direccion = await _direccionService.CrearDireccionAsync(usuarioId, dto);
                return Ok(direccion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpPut("actualizar/{id}")]
        [Authorize]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarDireccionDto dto)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                var direccion = await _direccionService.ActualizarDireccionAsync(usuarioId, id, dto);
                return Ok(direccion);
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

        [HttpDelete("eliminar/{id}")]
        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var token = authHeader.Substring("Bearer ".Length).Trim();
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                await _direccionService.EliminarDireccionAsync(usuarioId, id);
                return Ok(new { mensaje = "Dirección eliminada correctamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }
    }
}
