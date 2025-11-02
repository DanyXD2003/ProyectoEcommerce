using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface IDescuentoRepository
    {
        Task<IEnumerable<Descuento>> GetAllAsync();
        Task<Descuento?> GetByIdAsync(int id);
        Task<Descuento?> GetByCodigoAsync(string codigo);
        Task AddAsync(Descuento descuento);
        Task UpdateAsync(Descuento descuento);
        Task DeleteAsync(int id);
    }
}
