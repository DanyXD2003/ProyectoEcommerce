using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CarritoDetalleRepository : ICarritoDetalleRepository
    {
        private readonly EcommerceDbContext _context;
        public CarritoDetalleRepository(EcommerceDbContext context) => _context = context;

        // ---------- Mapeos ----------
        // Infra -> Dominio
        private static CarritoDetalle ToDomain(CarritoDetalle e) => new CarritoDetalle(
            e.Id,
            e.ProductoId,
            e.Cantidad,
            e.PrecioUnitario
        );

        // ---------- CRUD ----------
        public async Task<IReadOnlyList<CarritoDetalle>> ListByCarritoAsync(int carritoId)
        {
            var entities = await _context.Set<CarritoDetalle>()
                                         .AsNoTracking()
                                         .Where(x => x.Id == carritoId)
                                         .ToListAsync();

            return entities.Select(ToDomain).ToList();
        }

        public async Task AddAsync(CarritoDetalle detalle)
        {
            _context.Set<CarritoDetalle>().Add(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarritoDetalle detalle)
        {
            var entity = await _context.Set<CarritoDetalle>()
                                       .FirstOrDefaultAsync(x => x.Id == detalle.Id);
            if (entity is null)
                throw new InvalidOperationException("El detalle del carrito no existe.");

            entity = detalle; // Asignar los nuevos valores

            _context.Set<CarritoDetalle>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<CarritoDetalle>()
                                       .FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
                throw new InvalidOperationException("El detalle del carrito no existe.");

            _context.Set<CarritoDetalle>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }   
}