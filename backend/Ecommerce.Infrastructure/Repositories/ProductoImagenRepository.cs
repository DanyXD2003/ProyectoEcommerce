using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductoImagenRepository : IProductoImagenRepository
    {
        private readonly EcommerceDbContext _context;
        public ProductoImagenRepository(EcommerceDbContext context) => _context = context;

        private static ProductoImagen ToDomain(Infrastructure.Entities.producto_imagen e) =>
            new ProductoImagen(e.id, e.id_producto, e.url_imagen, e.fecha_subida);

        private static Infrastructure.Entities.producto_imagen ToEntity(ProductoImagen d) => new()
        {
            id = d.Id,
            id_producto = d.IdProducto,
            url_imagen = d.UrlImagen,
            fecha_subida = d.FechaSubida
        };

        public async Task<ProductoImagen?> GetByIdAsync(int id)
        {
            var e = await _context.producto_imagens.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(ProductoImagen productoImagen)
        {
            _context.producto_imagens.Add(ToEntity(productoImagen));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductoImagen productoImagen)
        {
            var e = ToEntity(productoImagen);
            _context.producto_imagens.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.producto_imagens.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.producto_imagens.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
