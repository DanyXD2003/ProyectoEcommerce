using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class DescuentoRepository : IDescuentoRepository
    {
        private readonly EcommerceDbContext _context;
        public DescuentoRepository(EcommerceDbContext context) => _context = context;

        private static Descuento ToDomain(Infrastructure.Entities.descuento e) =>
            new Descuento(e.id, e.codigo, e.porcentaje, e.inicio, e.fin, e.activo);

        private static Infrastructure.Entities.descuento ToEntity(Descuento d) => new()
        {
            id = d.Id,
            codigo = d.Codigo,
            porcentaje = d.Porcentaje,
            inicio = d.Inicio,
            fin = d.Fin,
            activo = d.Activo
        };

        public async Task<Descuento?> GetByCodigoAsync(string codigo)
        {
            var e = await _context.descuentos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.codigo == codigo);

            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Descuento d)
        {
            _context.descuentos.Add(ToEntity(d));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Descuento d)
        {
            var e = ToEntity(d);
            _context.descuentos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.descuentos.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.descuentos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
