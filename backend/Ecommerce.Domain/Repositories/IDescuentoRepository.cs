using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IDescuentoRepository
    {
        Task<Descuento?> GetByCodigoAsync(string codigo);
        Task AddAsync(Descuento descuento);
        Task UpdateAsync(Descuento descuento);
        Task DeleteAsync(int id);
    }
}
