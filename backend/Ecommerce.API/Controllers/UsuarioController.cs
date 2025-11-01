using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Domain.Repositories;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly JwtService _jwtService;

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(UsuarioService usuarioService, JwtService jwtService, IUsuarioRepository usuarioRepository)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
            _usuarioRepository = usuarioRepository;
        }

        // Endpoint POST api/usuario/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _usuarioService.LoginAsync(dto);

            if (token == null)
                return Unauthorized("Correo o contraseña incorrectos");

            /*// ✅ Recuperar datos del usuario para enviarlos al frontend
            var usuario = await _usuarioService.BuscarPorCorreoAsync(dto.Correo);*/

            return Ok(new { 
                Token = token,
                //Correo = usuario.Correo,
            });
        }


        // Endpoint POST api/usuario/registrar
        [HttpPost("registrar")]
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

        // Endpoint GET api/usuario/GetInfoByCorreo}
        [HttpGet("GetInfoByCorreo")]
        public async Task<IActionResult> GetInfoByCorreo(string correo)
        {
            var usuario = await _usuarioService.BuscarPorCorreoAsync(correo);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            return Ok(new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Apellido,
                usuario.Correo,
                usuario.Pedidos,
                usuario.Direcciones
            });
        } 

        // GET api/usuario/buscar
        [HttpGet("buscar")]
        [Authorize]
        public async Task<IActionResult> Buscar()
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    return Unauthorized(new { message = "Token no proporcionado o inválido" });

                var token = authHeader.Substring("Bearer ".Length).Trim();

                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var claimsDebug = jwt.Claims.Select(c => new { c.Type, c.Value }).ToList();

                var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized(new { message = "No se encontró el claim NameIdentifier en el token", claims = claimsDebug });

                if (!int.TryParse(userIdClaim.Value, out int userId))
                    return Unauthorized(new { message = "El claim NameIdentifier no es un número válido", claims = claimsDebug });

                var usuario = await _usuarioRepository.GetByIdAsync(userId);
                if (usuario == null)
                    return NotFound(new { message = "Usuario no encontrado", claims = claimsDebug });

                return Ok(new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Correo,
                    usuario.Rol,
                    usuario.FechaRegistro,
                    claims = claimsDebug  // opcional: retornar claims para depuración
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
