// Ecommerce.Application/Services/UsuarioService.cs
using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtGenerator;
        private readonly EcommerceDbContext _context; // agregado para consultas directas

        // Constructor con inyección de dependencias
        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IJwtTokenGenerator jwtGenerator,
            EcommerceDbContext context)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _jwtGenerator = jwtGenerator;
            _context = context;
        }

        // Login
        public async Task<string?> LoginAsync(UsuarioLoginDTO dto)
        {
            var usuario = await _usuarioRepository.GetByCorreoAsync(dto.Correo);
            if (usuario == null || usuario.ContrasenaHash != dto.Contrasena)
                return null;

            return _jwtGenerator.GenerateToken(usuario);
        }

        // Registro (cliente)
        public async Task<Usuario> RegistrarUsuarioAsync(UsuarioRegistroDTO dto)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(dto);
                usuario.FechaRegistro = DateTime.UtcNow;

                await _usuarioRepository.AddAsync(usuario);
                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR SQL] {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }

        // Buscar usuario por correo
        public async Task<UsuarioInfoDTO?> ObtenerInfoPorCorreoAsync(string correo)
        {
            var usuario = await _usuarioRepository.GetByCorreoAsync(correo);
            if (usuario == null)
                return null;

            return _mapper.Map<UsuarioInfoDTO>(usuario);
        }


        // Traer todos los usuarios (solo admin)
        public async Task<IEnumerable<UsuarioAdminDTO>> TraerTodosAsync()
        {
            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UsuarioAdminDTO>>(usuarios);
        }

        // Agregar un nuevo usuario (solo admin)
        public async Task<UsuarioAdminDTO> AgregarUsuarioAsync(UsuarioRegistroAdminDTO dto)
        {
            try
            {
                var usuario = _mapper.Map<Usuario>(dto);
                usuario.FechaRegistro = DateTime.UtcNow;

                await _usuarioRepository.AddAsync(usuario);
                return _mapper.Map<UsuarioAdminDTO>(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR SQL] {ex.InnerException?.Message ?? ex.Message}");
                throw;
            }
        }

        // Modificar usuario existente (solo admin)
        public async Task<UsuarioAdminDTO?> ModificarUsuarioAsync(int id, UsuarioActualizarDTO dto)
        {
            var usuarioExistente = await _usuarioRepository.GetByIdAsync(id);
            if (usuarioExistente == null)
                return null;

            _mapper.Map(dto, usuarioExistente);
            await _usuarioRepository.UpdateAsync(usuarioExistente);

            return _mapper.Map<UsuarioAdminDTO>(usuarioExistente);
        }

        // Eliminar usuario (solo admin)
        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return false;

            await _usuarioRepository.DeleteAsync(id);
            return true;
        }
    }
}
