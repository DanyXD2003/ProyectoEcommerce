using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IPagoRepository
    {
        Task<Pago?> GetByIdAsync(int id);
        Task<Pago?> GetByPedidoAsync(int pedidoId);
        Task AddAsync(Pago pago);
        Task UpdateAsync(Pago pago);
        Task DeleteAsync(int id);
    }
}