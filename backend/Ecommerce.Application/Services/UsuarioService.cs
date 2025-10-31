// Ecommerce.Application/Services/UsuarioService.cs
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;

using AutoMapper;

namespace Ecommerce.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtGenerator;

        // Constructor con inyección de dependencias
        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper, IJwtTokenGenerator jwtGenerator)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
        }

        // Método para login
        public async Task<string?> LoginAsync(UsuarioLoginDTO dto)
        {
            // Buscar usuario por correo
            var usuario = await _usuarioRepository.GetByCorreoAsync(dto.Correo);

            if (usuario == null)
                return null;

            // Validar contraseña (ejemplo simple, deberías usar hash en el futuro)
            if (usuario.ContrasenaHash != dto.Contrasena)
                return null;

            // Generar el token
            var token = _jwtGenerator.GenerateToken(usuario);

            return token;
        }


        // Método para registrar un nuevo usuario
        public async Task<Usuario> RegistrarUsuarioAsync(UsuarioRegistroDTO dto)
        {
            try {
                var usuario = _mapper.Map<Usuario>(dto);

                // Forzar la fecha como "local" (sin zona horaria)
                usuario.FechaRegistro = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

                await _usuarioRepository.AddAsync(usuario);
                return usuario;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[ERROR SQL] {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }
    }
}
