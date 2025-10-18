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

        private static Pago ToDomain(Infrastructure.Entities.pago e) =>
            new Pago(e.id, e.pedido_id, e.monto, e.fecha, e.estado, e.metodo_pago_id, e.transaccion_ref);

        private static Infrastructure.Entities.pago ToEntity(Pago d) => new()
        {
            id = d.Id,
            pedido_id = d.PedidoId,
            monto = d.Monto,
            fecha = d.Fecha,
            estado = d.Estado,
            metodo_pago_id = d.MetodoPagoId,
            transaccion_ref = d.TransaccionRef
        };

        public async Task<Pago?> GetByIdAsync(int id)
        {
            var e = await _context.pagos.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<Pago?> GetByPedidoAsync(int pedidoId)
        {
            var e = await _context.pagos.AsNoTracking().FirstOrDefaultAsync(x => x.pedido_id == pedidoId);
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
            var e = await _context.pagos.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.pagos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
