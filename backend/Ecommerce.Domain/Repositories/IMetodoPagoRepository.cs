using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface IMetodoPagoRepository
    {
        Task<IEnumerable<MetodoPago>> GetAllAsync();             
        Task<IEnumerable<MetodoPago>> GetByUsuarioAsync(int usuarioId); 
        Task<MetodoPago?> GetByIdAsync(int id);
        Task AddAsync(MetodoPago metodo);
        Task UpdateAsync(MetodoPago metodo);
        Task DeleteAsync(int id);
    }
}
