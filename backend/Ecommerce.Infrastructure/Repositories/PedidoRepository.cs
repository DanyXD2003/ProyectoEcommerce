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
            new Pedido(e.id, e.usuario_id, e.fecha, e.estado, e.total);

        private static Infrastructure.Entities.pedido ToEntity(Pedido d) => new()
        {
            id = d.Id,
            usuario_id = d.UsuarioId,
            fecha = d.Fecha,
            estado = d.Estado,
            total = d.Total
        };

        public async Task<Pedido?> GetByIdAsync(int id)
        {
            var e = await _context.pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<Pedido>> ListByUsuarioAsync(int usuarioId)
        {
            var list = await _context.pedidos.AsNoTracking()
                .Where(x => x.usuario_id == usuarioId)
                .OrderByDescending(x => x.fecha)
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
            var e = await _context.pedidos.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.pedidos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
