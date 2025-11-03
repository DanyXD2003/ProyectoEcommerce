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

        // Crear un pedido
        public async Task AddAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        // Obtener pedido por ID (con relaciones)
        public async Task<Pedido?> GetByIdAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(p => p.Direccion)
                .Include(p => p.MetodoPago)
                .Include(p => p.Usuario)
                .Include(p => p.Carrito)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Obtener todos los pedidos de un usuario
        public async Task<IEnumerable<Pedido>> GetByUsuarioAsync(int usuarioId)
        {
            return await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(p => p.Direccion)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.FechaPedido)
                .AsNoTracking()
                .ToListAsync();
        }

        // Obtener todos los pedidos (admin)
        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Direccion)
                .Include(p => p.MetodoPago)
                .OrderByDescending(p => p.FechaPedido)
                .AsNoTracking()
                .ToListAsync();
        }

        // Actualizar un pedido (estado, total, etc.)
        public async Task UpdateAsync(Pedido pedido)
        {
            _context.Pedidos.Attach(pedido);
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Eliminar pedido
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
