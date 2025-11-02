using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Services;
using Ecommerce.Application.DTOs;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescuentoController : ControllerBase
    {
        private readonly DescuentoService _descuentoService;

        public DescuentoController(DescuentoService descuentoService)
        {
            _descuentoService = descuentoService;
        }

        // 🔹 Obtener todos los descuentos (solo admin)
        [HttpGet("obtenerTodos")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var descuentos = await _descuentoService.GetAllAsync();
                return Ok(descuentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Obtener descuento por ID (solo admin)
        [HttpGet("obtenerPorId/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var descuento = await _descuentoService.GetByIdAsync(id);
                if (descuento == null)
                    return NotFound(new { mensaje = "Descuento no encontrado." });

                return Ok(descuento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Obtener descuento por código (público)
        [HttpGet("obtenerPorCodigo/{codigo}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorCodigo(string codigo)
        {
            try
            {
                var descuento = await _descuentoService.GetByCodigoAsync(codigo);
                if (descuento == null)
                    return NotFound(new { mensaje = "Descuento no encontrado o inactivo." });

                return Ok(descuento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Crear descuento (admin)
        [HttpPost("crear")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Crear([FromBody] CrearDescuentoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var descuento = await _descuentoService.CreateAsync(dto);
                return Ok(descuento);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Actualizar descuento (admin)
        [HttpPut("actualizar")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarDescuentoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var descuento = await _descuentoService.UpdateAsync(dto);
                return Ok(descuento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        // 🔹 Eliminar descuento (admin)
        [HttpDelete("eliminar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _descuentoService.DeleteAsync(id);
                return Ok(new { mensaje = "Descuento eliminado correctamente." });
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
    }
}
