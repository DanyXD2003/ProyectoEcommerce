using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class PedidoDetalleRepository : IPedidoDetalleRepository
    {
        private readonly EcommerceDbContext _context;
        public PedidoDetalleRepository(EcommerceDbContext context) => _context = context;

        private static PedidoDetalle ToDomain(PedidoDetalle e) =>
            new PedidoDetalle( e.Id, e.Cantidad, e.PrecioUnitario);

        public async Task<IReadOnlyList<PedidoDetalle>> ListByPedidoAsync(int pedidoId)
        {
            var list = await _context.PedidoDetalles
                .AsNoTracking()
                .Where(x => x.Id == pedidoId)
                .ToListAsync();

            return list.Select(ToDomain).ToList();
        }

        public async Task AddAsync(PedidoDetalle d)
        {
            _context.PedidoDetalles.Add(d);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PedidoDetalle d)
        {
            _context.PedidoDetalles.Attach(d);
            _context.Entry(d).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var detalle = await _context.PedidoDetalles.FindAsync(id);
            if (detalle != null)
            {
                _context.PedidoDetalles.Remove(detalle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
