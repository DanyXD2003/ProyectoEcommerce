using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Repositories;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioService usuarioService, IUsuarioRepository usuarioRepository)
        {
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _usuarioService.LoginAsync(dto);
            if (token == null)
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });

            return Ok(new { token });
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar([FromBody] UsuarioRegistroDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var usuario = await _usuarioService.RegistrarUsuarioAsync(dto);
                return Ok(new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Correo,
                    usuario.Rol
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpGet("GetInfoByCorreo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInfoByCorreo(string correo)
        {
            var usuario = await _usuarioService.ObtenerInfoPorCorreoAsync(correo);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            return Ok(usuario);
        }


        [HttpGet("buscar")]
        [Authorize]
        public async Task<IActionResult> Buscar()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { mensaje = "Token inválido o sin claim de usuario" });

                if (!int.TryParse(userIdClaim, out int userId))
                    return Unauthorized(new { mensaje = "El claim NameIdentifier no es válido" });

                var usuario = await _usuarioRepository.GetByIdAsync(userId);
                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Correo,
                    usuario.Rol,
                    usuario.FechaRegistro
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpGet("TraerTodos")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> TraerTodos()
        {
            try
            {
                var usuarios = await _usuarioService.TraerTodosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpPost("AgregarUsuario")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AgregarUsuario([FromBody] UsuarioRegistroAdminDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var nuevoUsuario = await _usuarioService.AgregarUsuarioAsync(dto);
                return Ok(nuevoUsuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpPut("ModificarUsuario/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ModificarUsuario(int id, [FromBody] UsuarioActualizarDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var actualizado = await _usuarioService.ModificarUsuarioAsync(id, dto);
                if (actualizado == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(actualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpDelete("EliminarUsuario/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                var eliminado = await _usuarioService.EliminarUsuarioAsync(id);
                if (!eliminado)
                    return NotFound(new { mensaje = "Usuario no encontrado" });

                return Ok(new { mensaje = "Usuario eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }
    }
}
