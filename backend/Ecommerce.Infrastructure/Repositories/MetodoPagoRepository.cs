using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly EcommerceDbContext _context;
        public MetodoPagoRepository(EcommerceDbContext context) => _context = context;

        private static MetodoPago ToDomain(Infrastructure.Entities.metodo_pago e) =>
            new MetodoPago(e.id, e.nombre, e.activo);

        private static Infrastructure.Entities.metodo_pago ToEntity(MetodoPago d) => new()
        {
            id = d.Id,
            nombre = d.Nombre,
            activo = d.Activo
        };

        public async Task<IReadOnlyList<MetodoPago>> ListActivosAsync()
        {
            var list = await _context.metodos_pago
                .AsNoTracking()
                .Where(x => x.activo)
                .OrderBy(x => x.nombre)
                .ToListAsync();

            return list.Select(ToDomain).ToList();
        }

        public async Task AddAsync(MetodoPago m)
        {
            _context.metodos_pago.Add(ToEntity(m));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MetodoPago m)
        {
            var e = ToEntity(m);
            _context.metodos_pago.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.metodos_pago.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.metodos_pago.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
