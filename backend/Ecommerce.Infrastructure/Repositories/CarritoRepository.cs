using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly EcommerceDbContext _context;
        public CarritoRepository(EcommerceDbContext context) => _context = context;

        // ---------- Mapeos ----------
        // Infra -> Dominio
        private static Carrito ToDomain(Carrito e, bool includeDetalles = false)
        {
            if (!includeDetalles)
                return new Carrito(e.Id, e.UsuarioId, e.FechaCreacion );

            var detalles = new List<CarritoDetalle>();
            foreach (var d in detalles)
            {
                //  Ajusta esta línea si tu CarritoDetalle tiene otra firma
                detalles.Add(new CarritoDetalle(d.Id, d.ProductoId, d.Cantidad, d.PrecioUnitario)
                );
            }

            return new Carrito(e.Id, e.UsuarioId, e.FechaCreacion, detalles);
        }


        // ---------- CRUD ----------
        public async Task<Carrito?> GetByIdAsync(int id)
        {
            var e = await _context.Set<Carrito>()
                                  .Include(x => x.Detalles)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.Id == id);

            return e is null ? null : ToDomain(e, includeDetalles: true);
        }

        public async Task<Carrito?> GetActivoByUsuarioAsync(int usuarioId)
        {
            // Por tu índice único en id_usuario, debe existir 0-1 carrito por usuario
            var e = await _context.Set<Carrito>()
                                  .Include(x => x.Detalles)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);

            return e is null ? null : ToDomain(e, includeDetalles: true);
        }

        public async Task AddAsync(Carrito c)
        {
            _context.Set<Carrito>().Add(c);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Carrito c)
        {
            _context.Set<Carrito>().Attach(c);
            _context.Entry(c).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.Set<Carrito>().FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return;
            _context.Set<Carrito>().Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
