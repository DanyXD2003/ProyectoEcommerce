using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Pedido>> ListByUsuarioAsync(Guid usuarioId);
        Task AddAsync(Pedido pedido);
        Task UpdateAsync(Pedido pedido);
        Task DeleteAsync(Guid id);
    }
}