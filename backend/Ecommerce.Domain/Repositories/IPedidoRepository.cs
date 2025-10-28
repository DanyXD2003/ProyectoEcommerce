using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(int id);
        Task<IReadOnlyList<Pedido>> ListByUsuarioAsync(int usuarioId);
        Task AddAsync(Pedido pedido);
        Task UpdateAsync(Pedido pedido);
        Task DeleteAsync(int id);
    }
}