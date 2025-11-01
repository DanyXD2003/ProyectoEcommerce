using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        //Obtener todos los productos activos (público)
        [HttpGet("obtenerTodos")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var productos = await _productoService.ObtenerTodosAsync();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        //Obtener todos los productos (admin)
        [HttpGet("obtenerTodosAdmin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ObtenerTodosAdmin()
        {
            try
            {
                var productos = await _productoService.ObtenerTodosAdminAsync();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        //Obtener producto por ID (público)
        [HttpGet("obtenerPorId/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerPorIdAsync(id);
                if (producto == null)
                    return NotFound(new { mensaje = "Producto no encontrado" });

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        //Obtener productos por categoría (público)
        [HttpGet("obtenerPorCategoria/{categoriaId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerPorCategoria(int categoriaId)
        {
            try
            {
                var productos = await _productoService.ObtenerPorCategoriaAsync(categoriaId);
                if (!productos.Any())
                    return NotFound(new { mensaje = "No se encontraron productos para esta categoría" });

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        //Crear producto (solo admin)
        [HttpPost("crearProducto")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoDto dto)
        {
            try
            {
                var producto = await _productoService.CrearProductoAsync(dto);
                return Ok(producto);
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

        //Actualizar producto (solo admin)
        [HttpPut("actualizarProducto/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] ActualizarProductoDto dto)
        {
            try
            {
                var producto = await _productoService.ActualizarProductoAsync(id, dto);
                return Ok(producto);
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

        //Eliminar producto (borrado lógico, solo admin)
        [HttpDelete("eliminarProducto/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                await _productoService.EliminarProductoAsync(id);
                return Ok(new { mensaje = "Producto eliminado correctamente" });
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
