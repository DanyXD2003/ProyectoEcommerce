using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;


namespace Ecommerce.Domain.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> GetByIdAsync(int id);
        Task AddAsync(Categoria categoria);
        Task UpdateAsync(Categoria categoria);
        Task DeleteAsync(int id);
    }
}
