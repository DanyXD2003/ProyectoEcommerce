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

        private static PedidoDetalle ToDomain(Infrastructure.Entities.pedido_detalle e) =>
            new PedidoDetalle(e.id, e.pedido_id, e.producto_id, e.cantidad, e.precio_unitario);

        private static Infrastructure.Entities.pedido_detalle ToEntity(PedidoDetalle d) => new()
        {
            id = d.Id,
            pedido_id = d.PedidoId,
            producto_id = d.ProductoId,
            cantidad = d.Cantidad,
            precio_unitario = d.PrecioUnitario
        };

        public async Task<IReadOnlyList<PedidoDetalle>> ListByPedidoAsync(int pedidoId)
        {
            var list = await _context.pedido_detalles
                .AsNoTracking()
                .Where(x => x.pedido_id == pedidoId)
                .ToListAsync();

            return list.Select(ToDomain).ToList();
        }

        public async Task AddAsync(PedidoDetalle d)
        {
            _context.pedido_detalles.Add(ToEntity(d));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PedidoDetalle d)
        {
            var e = ToEntity(d);
            _context.pedido_detalles.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.pedido_detalles.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.pedido_detalles.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
