// Ecommerce.API/Controllers/UsuarioController.cs
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Application.DTOs;
using Ecommerce.Application.Services;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
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

            return Ok(new { Token = token });
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
    }
}
