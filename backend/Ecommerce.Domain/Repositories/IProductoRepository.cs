using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IProductoRepository
    {
        Task<Producto?> GetByIdAsync(int id);
        Task AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task DeleteAsync(int id);
    }
}
