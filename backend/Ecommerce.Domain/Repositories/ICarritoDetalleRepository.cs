using Ecommerce.Domain.Entities;
namespace Ecommerce.Domain.Repositories
{
    public interface ICarritoDetalleRepository
    {
        Task<IReadOnlyList<CarritoDetalle>> ListByCarritoAsync(int carritoId);
        Task AddAsync(CarritoDetalle detalle);
        Task UpdateAsync(CarritoDetalle detalle);
        Task DeleteAsync(int id);
    }
}