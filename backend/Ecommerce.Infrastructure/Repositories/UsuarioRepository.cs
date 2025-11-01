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

private static Usuario ToDomain(Usuario e)
{
    // Crear el usuario base con el constructor de rehidratación
    var usuario = new Usuario(
        e.Nombre,
        e.Apellido,
        e.Correo,
        e.ContrasenaHash,
        e.Rol
    );

    // Rehidratar las relaciones si existen
    if (e.Direcciones != null && e.Direcciones.Any())
    {
        foreach (var d in e.Direcciones)
        {
            usuario.Direcciones.Add(new Direccion(
                d.Id,
                d.UsuarioId,
                d.Calle,
                d.Ciudad,
                d.Departamento,
                d.CodigoPostal,
                d.Pais,
                d.Telefono
            ));
        }
    }

    if (e.MetodosPago != null && e.MetodosPago.Any())
    {
        foreach (var mp in e.MetodosPago)
        {
            usuario.MetodosPago.Add(mp); // Ajusta si tu clase dominio necesita un constructor específico
        }
    }

    if (e.Pedidos != null && e.Pedidos.Any())
    {
        foreach (var p in e.Pedidos)
        {
            usuario.Pedidos.Add(p);
        }
    }

    return usuario;
}

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            var e = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<Usuario?> GetByCorreoAsync(string correo)
        {
            var e = await _context.Usuarios
                .Include(u => u.Direcciones)
                .Include(u => u.MetodosPago)
                .Include(u => u.Pedidos)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Correo == correo);

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
