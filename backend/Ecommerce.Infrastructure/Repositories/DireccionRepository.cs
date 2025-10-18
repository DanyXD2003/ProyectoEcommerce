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

        private static Direccion ToDomain(Infrastructure.Entities.direccion e) =>
            new Direccion(e.id, e.usuario_id, e.linea1, e.linea2, e.ciudad, e.estado, e.cp, e.pais);

        private static Infrastructure.Entities.direccion ToEntity(Direccion d) => new()
        {
            id = d.Id,
            usuario_id = d.UsuarioId,
            linea1 = d.Linea1,
            linea2 = d.Linea2,
            ciudad = d.Ciudad,
            estado = d.Estado,
            cp = d.CP,
            pais = d.Pais
        };

        public async Task<IReadOnlyList<Direccion>> ListByUsuarioAsync(int usuarioId)
        {
            var list = await _context.direcciones
                .AsNoTracking()
                .Where(x => x.usuario_id == usuarioId)
                .ToListAsync();

            return list.Select(ToDomain).ToList();
        }

        public async Task AddAsync(Direccion d)
        {
            _context.direcciones.Add(ToEntity(d));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Direccion d)
        {
            var e = ToEntity(d);
            _context.direcciones.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.direcciones.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.direcciones.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
