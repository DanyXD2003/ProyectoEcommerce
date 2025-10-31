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

        private static Pedido ToDomain(Pedido e) =>
            new Pedido(e.Id, e.DireccionId, e.MetodoPagoId);

        public async Task<Pedido?> GetByIdAsync(int id)
        {
            var e = await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<IReadOnlyList<Pedido>> ListByUsuarioAsync(int usuarioId)
        {
            var list = await _context.Pedidos
                .AsNoTracking()
                .Where(x => x.Id == usuarioId)
                .ToListAsync();

            return list.Select(ToDomain).ToList();
        }

        public async Task AddAsync(Pedido p)
        {
            _context.Pedidos.Add(p);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pedido p)
        {
            _context.Pedidos.Attach(p);
            _context.Entry(p).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
        }
    }
}
