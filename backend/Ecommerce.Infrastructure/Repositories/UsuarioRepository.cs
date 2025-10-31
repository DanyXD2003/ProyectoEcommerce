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

        private static Usuario ToDomain(Usuario e) =>
            // usar el constructor de rehidratación del dominio
            new Usuario(
                e.Id,
                e.Nombre,
                e.Apellido,
                e.Correo,
                e.ContrasenaHash,
                e.Telefono,
                e.Rol,
                e.FechaRegistro);

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            var e = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            var e = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Correo == correo);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Attach(usuario);
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
