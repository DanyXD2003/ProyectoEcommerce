using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface IPedidoDetalleRepository
    {
        // Obtener detalles de un pedido
        Task<IEnumerable<PedidoDetalle>> GetByPedidoIdAsync(int pedidoId);

        // Agregar detalles a un pedido
        Task AddRangeAsync(IEnumerable<PedidoDetalle> detalles);
    }
}
