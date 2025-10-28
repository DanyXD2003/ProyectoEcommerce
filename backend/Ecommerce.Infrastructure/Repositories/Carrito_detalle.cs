using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;


namespace Ecommerce.Infrastructure.Repositories
{
    public class CarritoDetalleRepository : ICarritoDetalleRepository
    {
        private readonly EcommerceDbContext _context;
        public CarritoDetalleRepository(EcommerceDbContext context) => _context = context;

        // ---------- Mapeos ----------
        // Infra -> Dominio
        private static CarritoDetalle ToDomain(carrito_detalle e) => new CarritoDetalle(
            e.id_carrito,
            e.id_producto,
            e.cantidad,
            e.precio_unitario
        );

        // Dominio -> Infra
        private static carrito_detalle ToEntity(CarritoDetalle d) => new carrito_detalle
        {
            id_carrito = d.CarritoId,
            id_producto = d.ProductoId,
            cantidad = d.Cantidad,
            precio_unitario = d.PrecioUnitario
        };

        // ---------- CRUD ----------
        public async Task<IReadOnlyList<CarritoDetalle>> ListByCarritoAsync(int carritoId)
        {
            var entities = await _context.Set<carrito_detalle>()
                                         .AsNoTracking()
                                         .Where(x => x.id_carrito == carritoId)
                                         .ToListAsync();

            return entities.Select(ToDomain).ToList();
        }

        public async Task AddAsync(CarritoDetalle detalle)
        {
            var entity = ToEntity(detalle);
            await _context.Set<carrito_detalle>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarritoDetalle detalle)
        {
            var entity = await _context.Set<carrito_detalle>()
                                       .FirstOrDefaultAsync(x => x.id_carrito == detalle.CarritoId && x.id_producto == detalle.ProductoId);
            if (entity is null)
                throw new InvalidOperationException("El detalle del carrito no existe.");

            entity.cantidad = detalle.Cantidad;
            entity.precio_unitario = detalle.PrecioUnitario;

            _context.Set<carrito_detalle>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<carrito_detalle>()
                                       .FirstOrDefaultAsync(x => x.id_carrito == id);
            if (entity is null)
                throw new InvalidOperationException("El detalle del carrito no existe.");

            _context.Set<carrito_detalle>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }   
}