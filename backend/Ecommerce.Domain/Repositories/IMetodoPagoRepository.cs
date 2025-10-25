using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IMetodoPagoRepository
    {
        Task<IReadOnlyList<MetodoPago>> ListActivosAsync();
        Task AddAsync(MetodoPago metodo);
        Task UpdateAsync(MetodoPago metodo);
        Task DeleteAsync(int id);
    }
}
