using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritoController : ControllerBase
    {
        private readonly CarritoService _carritoService;
        private readonly JwtService _jwtService;

        public CarritoController(CarritoService carritoService, JwtService jwtService)
        {
            _carritoService = carritoService;
            _jwtService = jwtService;
        }

        //Obtener carrito activo del usuario
        [HttpGet("obtenerActivo")]
        [Authorize]
        public async Task<IActionResult> ObtenerActivo()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                var carrito = await _carritoService.ObtenerCarritoActivoAsync(usuarioId);
                if (carrito == null)
                    return NotFound(new { mensaje = "No hay un carrito activo para este usuario." });

                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        //Agregar producto al carrito
        [HttpPost("agregarProducto")]
        [Authorize]
        public async Task<IActionResult> AgregarProducto([FromBody] AgregarProductoDto dto)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                var detalle = await _carritoService.AgregarProductoAsync(usuarioId, dto);
                return Ok(detalle);
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

        //Eliminar un producto específico del carrito
        [HttpDelete("eliminarProducto/{productoId}")]
        [Authorize]
        public async Task<IActionResult> EliminarProducto(int productoId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                await _carritoService.EliminarProductoAsync(usuarioId, productoId);
                return Ok(new { mensaje = "Producto eliminado del carrito." });
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

        //Vaciar carrito (eliminar todos los productos)
        [HttpDelete("vaciar")]
        [Authorize]
        public async Task<IActionResult> VaciarCarrito()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                await _carritoService.VaciarCarritoAsync(usuarioId);
                return Ok(new { mensaje = "Carrito vaciado correctamente." });
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

        //Actualizar la cantidad de un producto en el carrito
        [HttpPut("actualizarCantidad")]
        [Authorize]
        public async Task<IActionResult> ActualizarCantidad([FromBody] ActualizarCantidadDto dto)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                var detalleActualizado = await _carritoService.ActualizarCantidadProductoAsync(usuarioId, dto);
                return Ok(detalleActualizado);
            }
            catch (InvalidOperationException ex)
            {
                // Caso especial: se eliminó por cantidad 0
                return Ok(new { mensaje = ex.Message });
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

        //Aplicar un descuento al carrito activo
        [HttpPost("aplicarDescuento")]
        [Authorize]
        public async Task<IActionResult> AplicarDescuento([FromBody] AplicarDescuentoDto dto)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token) ?? throw new UnauthorizedAccessException("Token inválido");

                var carritoActualizado = await _carritoService.AplicarDescuentoAsync(usuarioId, dto.Codigo);
                return Ok(carritoActualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
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
    }
}
