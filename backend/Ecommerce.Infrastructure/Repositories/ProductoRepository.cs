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

        private static Producto ToDomain(Infrastructure.Entities.producto e) =>
            new Producto(e.id, e.nombre, e.descripcion, e.precio, e.stock, e.fecha_creacion);

        private static Infrastructure.Entities.producto ToEntity(Producto d) => new()
        {
            id = d.Id,
            nombre = d.Nombre,
            descripcion = d.Descripcion,
            precio = d.Precio,
            stock = d.Stock,
            fecha_creacion = d.FechaCreacion
        };

        public async Task<Producto?> GetByIdAsync(int id)
        {
            var e = await _context.productos.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Producto producto)
        {
            _context.productos.Add(ToEntity(producto));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            var e = ToEntity(producto);
            _context.productos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.productos.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.productos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}