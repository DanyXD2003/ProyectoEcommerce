using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly EcommerceDbContext _context;
        public PedidoRepository(EcommerceDbContext context) => _context = context;

        private static Pedido ToDomain(Infrastructure.Entities.pedido e) =>
            new Pedido(e.id_usuario, e.id_direccion, e.id_metodo_pago);

        private static Infrastructure.Entities.pedido ToEntity(Pedido d) => new()
        {
            id_usuario = d.UsuarioId,
            id_direccion = d.DireccionId,
            id_metodo_pago = d.MetodoPagoId,
            fecha_pedido = d.FechaPedido,  
            total = d.Total
        };

        public async Task<Pedido?> GetByIdAsync(int id)
        {
            var e = await _context.pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.id_pedido == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<Pedido>> ListByUsuarioAsync(int usuarioId)
        {
            var list = await _context.pedidos.AsNoTracking()
                .Where(x => x.id_usuario == usuarioId)
                .OrderByDescending(x => x.fecha_pedido)
                .ToListAsync();

            return list.Select(ToDomain).ToList();
        }

        public async Task AddAsync(Pedido p)
        {
            _context.pedidos.Add(ToEntity(p));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pedido p)
        {
            var e = ToEntity(p);
            _context.pedidos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.pedidos.FirstOrDefaultAsync(x => x.id_usuario == id);
            if (e is null) return;
            _context.pedidos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
