using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly EcommerceDbContext _context;
        public PagoRepository(EcommerceDbContext context) => _context = context;

        private static Pago ToDomain(Pago e) =>
                new Pago(
                    e.Id,
                    e.Monto,
                    e.Metodo ?? string.Empty,
                    e.Estado ?? "Pendiente"
                );

        public async Task<Pago?> GetByIdAsync(int id)
        {
            var e = await _context.Pagos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<Pago?> GetByPedidoAsync(int pedidoId)
        {
            var e = await _context.Pagos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == pedidoId);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Pago p)
        {
            _context.Pagos.Add(ToDomain(p));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pago p)
        {
            var e = ToDomain(p);
            _context.Pagos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();
            }
        }
    }
}
