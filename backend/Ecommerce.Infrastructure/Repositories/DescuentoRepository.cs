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

        private static Descuento ToDomain(Descuento e) =>
            new Descuento(e.Codigo, e.Porcentaje, e.FechaInicio, e.FechaFin, e.Descripcion, e.Activo)
            {
                
            };


        public async Task<Descuento?> GetByCodigoAsync(string codigo)
        {
            var e = await _context.Descuentos.AsNoTracking().FirstOrDefaultAsync(d => d.Codigo == codigo);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Descuento d)
        {
            _context.Descuentos.Add(ToDomain(d));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Descuento d)
        {
            var e = ToDomain(d);
            _context.Descuentos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var descuento = await _context.Descuentos.FindAsync(id);
            if (descuento != null)
            {
                _context.Descuentos.Remove(descuento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
