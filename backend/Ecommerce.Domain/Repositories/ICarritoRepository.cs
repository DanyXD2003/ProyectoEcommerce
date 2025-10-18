using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface ICarritoRepository
    {
        Task<Carrito?> GetByIdAsync(Guid id);
        Task<Carrito?> GetActivoByUsuarioAsync(Guid usuarioId);
        Task AddAsync(Carrito carrito);
        Task UpdateAsync(Carrito carrito);
        Task DeleteAsync(Guid id);
    }
}
