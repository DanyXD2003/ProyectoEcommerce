using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.Repositories
{
    public interface IPedidoRepository
    {
        // Crear un nuevo pedido
        Task AddAsync(Pedido pedido);

        // Obtener pedido por ID
        Task<Pedido?> GetByIdAsync(int id);

        // Obtener todos los pedidos de un usuario
        Task<IEnumerable<Pedido>> GetByUsuarioAsync(int usuarioId);

        // Obtener todos los pedidos (admin)
        Task<IEnumerable<Pedido>> GetAllAsync();

        // Actualizar estado o información del pedido
        Task UpdateAsync(Pedido pedido);

        // Eliminar un pedido (solo admin o mantenimiento)
        Task DeleteAsync(int id);
    }
}
