using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly EcommerceDbContext _context;
        public PagoRepository(EcommerceDbContext context) => _context = context;

     private static Pago ToDomain(Infrastructure.Entities.pago e) =>
            new Pago(
                e.id_pedido,
                e.monto,
                e.metodo ?? string.Empty,
                e.estado_pago ?? "Pendiente"
            );

        private static Infrastructure.Entities.pago ToEntity(Pago d) => new()
        {
            id_pedido = d.PedidoId,
            monto = d.Monto,
            fecha_pago = d.FechaPago,
            estado_pago = d.Estado,
            metodo = d.Metodo,
        };

        public async Task<Pago?> GetByIdAsync(int id)
        {
            var e = await _context.pagos.AsNoTracking().FirstOrDefaultAsync(x => x.id_pago == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<Pago?> GetByPedidoAsync(int pedidoId)
        {
            var e = await _context.pagos.AsNoTracking().FirstOrDefaultAsync(x => x.id_pedido == pedidoId);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Pago p)
        {
            _context.pagos.Add(ToEntity(p));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pago p)
        {
            var e = ToEntity(p);
            _context.pagos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.pagos.FirstOrDefaultAsync(x => x.id_pago == id);
            if (e is null) return;
            _context.pagos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
