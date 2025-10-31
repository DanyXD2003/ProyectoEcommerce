using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;   
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly EcommerceDbContext _context;
        public ProductoRepository(EcommerceDbContext context) => _context = context;

        private static Producto ToDomain(Producto e) =>
            new Producto(
                e.CategoriaId,
                e.Nombre,
                e.Precio,
                e.Stock,
                e.Activo,
                e.Descripcion
            );

        public async Task<Producto?> GetByIdAsync(int id)
        {
            var e = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            _context.Productos.Attach(producto);
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }
    }
}