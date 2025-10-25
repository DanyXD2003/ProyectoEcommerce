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
            new Direccion(e.id_direccion, e.calle, e.ciudad, e.departamento, e.codigo_postal, e.pais);

        private static Infrastructure.Entities.direccion ToEntity(Direccion d) => new()
        {
            id_direccion = d.Id,
            id_usuario = d.UsuarioId,
            calle = d.Calle,
            ciudad = d.Ciudad,
            departamento = d.Departamento,
            codigo_postal = d.CodigoPostal,
            pais = d.Pais
        };

        public async Task<IReadOnlyList<Direccion>> ListByUsuarioAsync(int usuarioId)
        {
            var list = await _context.direccions
                .AsNoTracking()
                .Where(x => x.id_usuario == usuarioId)
                .ToListAsync();

            return list.Select(ToDomain).ToList();
        }

        public async Task AddAsync(Direccion d)
        {
            _context.direccions.Add(ToEntity(d));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Direccion d)
        {
            var e = ToEntity(d);
            _context.direccions.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.direccions.FirstOrDefaultAsync(x => x.id_direccion == id);
            if (e is null) return;
            _context.direccions.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
