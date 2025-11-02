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

        // Obtener los detalles de un pedido específico
        public async Task<IEnumerable<PedidoDetalle>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.PedidoDetalles
                .Include(d => d.Producto)
                .Where(d => d.PedidoId == pedidoId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Agregar un conjunto de detalles a un pedido
        public async Task AddRangeAsync(IEnumerable<PedidoDetalle> detalles)
        {
            await _context.PedidoDetalles.AddRangeAsync(detalles);
            await _context.SaveChangesAsync();
        }
    }
}
