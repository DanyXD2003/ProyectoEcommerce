using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IProductoImagenRepository
    {
        Task<IReadOnlyList<ProductoImagen>> ListByProductoAsync(Guid productoId);
        Task AddAsync(ProductoImagen imagen);
        Task DeleteAsync(Guid id);
    }
}
