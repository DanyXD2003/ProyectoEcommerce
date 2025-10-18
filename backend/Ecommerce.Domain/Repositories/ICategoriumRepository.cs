using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface ICategorumRepository
    {
        Task<Categoria?> GetByIdAsync(Guid id);
        Task AddAsync(Categoria categoria);
        Task UpdateAsync(Categoria categoria);
        Task DeleteAsync(Guid id);
    }
}
