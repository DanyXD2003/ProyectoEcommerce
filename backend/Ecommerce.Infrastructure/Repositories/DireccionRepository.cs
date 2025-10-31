using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class DireccionRepository : IDireccionRepository
    {
        private readonly EcommerceDbContext _context;
        public DireccionRepository(EcommerceDbContext context) => _context = context;

        private static Direccion ToDomain(Direccion e) =>
            new Direccion(e.Id, e.Calle, e.Ciudad, e.Departamento, e.CodigoPostal, e.Pais);


        public async Task<IReadOnlyList<Direccion>> ListByUsuarioAsync(int usuarioId)
        {
            return await _context.Direcciones
                .AsNoTracking()
                .Where(d => d.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task AddAsync(Direccion d)
        {
            _context.Direcciones.Add(d);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Direccion d)
        {
            _context.Direcciones.Attach(d);
            _context.Entry(d).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion != null)
            {
                _context.Direcciones.Remove(direccion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
