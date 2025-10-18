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
        public UsuarioRepository(EcommerceDbContext context) => _context = context;

        private static Usuario ToDomain(Infrastructure.Entities.usuario e) =>
            new Usuario(e.id, e.nombre, e.correo, e.contraseña_hash, e.rol, e.fecha_registro);

        private static Infrastructure.Entities.usuario ToEntity(Usuario d) => new()
        {
            id = d.Id,
            nombre = d.Nombre,
            correo = d.Correo,
            contraseña_hash = d.ContrasenaHash,
            rol = d.Rol,
            fecha_registro = d.FechaRegistro
        };

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            var e = await _context.usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            var e = await _context.usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.correo == correo);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Usuario usuario)
        {
            _context.usuarios.Add(ToEntity(usuario));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            var e = ToEntity(usuario);
            _context.usuarios.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.usuarios.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.usuarios.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
