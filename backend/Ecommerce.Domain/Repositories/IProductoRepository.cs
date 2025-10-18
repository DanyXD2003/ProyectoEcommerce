using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IProductoRepository
    {
        Task<Producto?> GetByIdAsync(Guid id);
        Task AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task DeleteAsync(Guid id);
    }
}
