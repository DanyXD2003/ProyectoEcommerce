using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

       private static ProductoImagen ToDomain(Ecommerce.Infrastructure.Entities.producto_imagen e) =>
            // Ajusta al constructor REAL de tu entidad de dominio.
            // Si tu dominio es: new ProductoImagen(int productoId, string url)
            new ProductoImagen(e.id_producto, e.url_imagen);

        private static Infrastructure.Entities.producto_imagen ToEntity(ProductoImagen d) => new()
        {
            id_producto = d.ProductoId,
            url_imagen = d.Url  
        };

    public async Task<IReadOnlyList<ProductoImagen>> ListByProductoAsync(int productoId)
        {
            var filas = await _context.producto_imagens
                                      .AsNoTracking()
                                      .Where(x => x.id_producto == productoId)
                                      .OrderBy(x => x.id_imagen)
                                      .ToListAsync();

            // List<T> implementa IReadOnlyList<T>
            return filas.Select(ToDomain).ToList();
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
            var e = await _context.producto_imagens.FirstOrDefaultAsync(x => x.id_imagen == id);
            if (e is null) return;
            _context.producto_imagens.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
