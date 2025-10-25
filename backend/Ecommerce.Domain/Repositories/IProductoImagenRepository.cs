using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface IProductoImagenRepository
    {
        Task<IReadOnlyList<ProductoImagen>> ListByProductoAsync(int productoId);
        Task AddAsync(ProductoImagen imagen);
        Task DeleteAsync(int id);
    }
}
