using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IPedidoDetalleRepository
    {
        Task<IReadOnlyList<PedidoDetalle>> ListByPedidoAsync(Guid pedidoId);
        Task AddAsync(PedidoDetalle detalle);
        Task UpdateAsync(PedidoDetalle detalle);
        Task DeleteAsync(Guid id);
    }
}
