using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.Services;
using Ecommerce.Application.DTOs;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;
        private readonly JwtService _jwtService;

        public PedidoController(PedidoService pedidoService, JwtService jwtService)
        {
            _pedidoService = pedidoService;
            _jwtService = jwtService;
        }

        // Crear un pedido a partir del carrito activo.
        [HttpPost("crear")]
        [Authorize]
        public async Task<IActionResult> CrearPedido([FromBody] CrearPedidoDto dto)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token)
                    ?? throw new UnauthorizedAccessException("Token inválido.");

                var pedido = await _pedidoService.CrearPedidoAsync(usuarioId, dto);
                return Ok(pedido);
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
                return StatusCode(500, new { mensaje = "Error al crear el pedido", detalle = ex.Message });
            }
        }

        // Obtener todos los pedidos del usuario autenticado.
        [HttpGet("misPedidos")]
        [Authorize]
        public async Task<IActionResult> MisPedidos()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token)
                    ?? throw new UnauthorizedAccessException("Token inválido.");

                var pedidos = await _pedidoService.ObtenerPedidosUsuarioAsync(usuarioId);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener los pedidos", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtener un pedido específico del usuario autenticado.
        /// </summary>
        [HttpGet("{pedidoId}")]
        [Authorize]
        public async Task<IActionResult> ObtenerPedido(int pedidoId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var usuarioId = _jwtService.GetUserIdFromToken(token)
                    ?? throw new UnauthorizedAccessException("Token inválido.");

                var pedido = await _pedidoService.ObtenerPedidoPorIdAsync(usuarioId, pedidoId);
                return Ok(pedido);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener el pedido", detalle = ex.Message });
            }
        }

        // Obtener todos los pedidos (solo administradores).
        [HttpGet("admin/listar")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListarTodos()
        {
            try
            {
                var pedidos = await _pedidoService.ObtenerTodosAsync();
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al listar pedidos", detalle = ex.Message });
            }
        }


        // Obtener un pedido específico (solo administradores).
        [HttpGet("admin/obtener/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ObtenerPorIdAdmin(int id)
        {
            try
            {
                var pedido = await _pedidoService.ObtenerPorIdAdminAsync(id);
                return Ok(pedido);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener el pedido", detalle = ex.Message });
            }
        }


        // Actualizar estado del pedido (solo administradores).
        [HttpPut("admin/actualizarEstado")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ActualizarEstado([FromBody] ActualizarEstadoPedidoDto dto)
        {
            try
            {
                var actualizado = await _pedidoService.ActualizarEstadoAsync(dto);
                return Ok(actualizado);
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
                return StatusCode(500, new { mensaje = "Error al actualizar el estado del pedido", detalle = ex.Message });
            }
        }


        // Eliminar un pedido (solo administradores).
        [HttpDelete("admin/eliminar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarPedido(int id)
        {
            try
            {
                await _pedidoService.EliminarAsync(id);
                return Ok(new { mensaje = "Pedido eliminado correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar el pedido", detalle = ex.Message });
            }
        }
    }
}
