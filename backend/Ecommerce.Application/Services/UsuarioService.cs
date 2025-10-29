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

        // Constructor con inyección de dependencias
        public UsuarioService(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        // Método para login
        public async Task<Usuario?> LoginAsync(UsuarioLoginDTO dto)
        {
            // Buscar usuario por correo
            var usuario = await _usuarioRepository.GetByCorreoAsync(dto.Correo);

            if (usuario == null)
                return null; // usuario no encontrado

            // Validar contraseña (ejemplo simple, más adelante con hash seguro)
            if (usuario.ContrasenaHash != dto.Contrasena)
                return null; // contraseña incorrecta

            // Retornar la entidad de dominio si todo es correcto
            return usuario;
        }

        // Método para registrar un nuevo usuario
        public async Task<Usuario> RegistrarUsuarioAsync(UsuarioRegistroDTO dto)
        {
            // Convertir DTO a dominio usando AutoMapper
            var usuario = _mapper.Map<Usuario>(dto);

            // Guardar en la base de datos
            await _usuarioRepository.AddAsync(usuario);

            return usuario;
        }
    }
}
