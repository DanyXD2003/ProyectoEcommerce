using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IDireccionRepository
    {
        Task<IReadOnlyList<Direccion>> ListByUsuarioAsync(Guid usuarioId);
        Task AddAsync(Direccion direccion);
        Task UpdateAsync(Direccion direccion);
        Task DeleteAsync(Guid id);
    }
}
