using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface IDireccionRepository
    {
        Task<IReadOnlyList<Direccion>> ListByUsuarioAsync(int usuarioId);
        Task AddAsync(Direccion direccion);
        Task UpdateAsync(Direccion direccion);
        Task DeleteAsync(int id);
    }
}
