using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IPagoRepository
    {
        Task<Pago?> GetByIdAsync(Guid id);
        Task<Pago?> GetByPedidoAsync(Guid pedidoId);
        Task AddAsync(Pago pago);
        Task UpdateAsync(Pago pago);
        Task DeleteAsync(Guid id);
    }
}