// Ecommerce.Infrastructure/Repositories/UsuarioRepository.cs
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly EcommerceDbContext _context;

        public UsuarioRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            var entity = await _context.usuarios
                .FirstOrDefaultAsync(u => u.correo == correo);

            if (entity == null)
                return null;

            // Aquí hacemos el mapeo manual de la entidad (DB) al dominio
            return new Usuario(entity.nombre, entity.correo, entity.contraseña_hash, entity.rol)
            {
                // Podés setear Id y FechaRegistro si querés mantenerlos
                // Id = entity.Id,
                // FechaRegistro = entity.FechaRegistro
            };
        }

        public async Task AddAsync(Usuario usuario)
        {
            var entity = new Infrastructure.Entities.usuario
            {
                nombre = usuario.Nombre,
                correo = usuario.Correo,
                contraseña_hash = usuario.ContrasenaHash,
                rol = usuario.Rol,
                fecha_registro = usuario.FechaRegistro
            };

            _context.usuarios.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
