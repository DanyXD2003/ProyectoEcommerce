using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface ICarritoRepository
    {
        Task<Carrito?> GetByIdAsync(int id);
        Task<Carrito?> GetActivoByUsuarioAsync(int usuarioId);
        Task AddAsync(Carrito carrito);
        Task UpdateAsync(Carrito carrito);
        Task DeleteAsync(int id);
    }
}
