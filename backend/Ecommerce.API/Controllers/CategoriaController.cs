using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet("obtenerTodas")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ObtenerTodas()
        {
            try
            {
                var categorias = await _categoriaService.ObtenerTodasAsync();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener las categorías", detalle = ex.Message });
            }
        }

        [HttpGet("obtenerPorId/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var categoria = await _categoriaService.ObtenerPorIdAsync(id);
                if (categoria == null)
                    return NotFound(new { mensaje = "Categoría no encontrada." });

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener la categoría", detalle = ex.Message });
            }
        }

        [HttpPost("crear")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var categoria = await _categoriaService.CrearAsync(dto);
                return Ok(categoria);
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
                return StatusCode(500, new { mensaje = "Error al crear la categoría", detalle = ex.Message });
            }
        }

        [HttpPut("actualizar")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarCategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var categoria = await _categoriaService.ActualizarAsync(dto);
                return Ok(categoria);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar la categoría", detalle = ex.Message });
            }
        }

        [HttpDelete("eliminar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _categoriaService.EliminarAsync(id);
                return Ok(new { mensaje = "Categoría eliminada correctamente." });
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
                return StatusCode(500, new { mensaje = "Error al eliminar la categoría", detalle = ex.Message });
            }
        }
    }
}
