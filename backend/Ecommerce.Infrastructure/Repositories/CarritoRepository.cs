using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Entities;
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
        private static Carrito ToDomain(carrito e, bool includeDetalles = false)
        {
            if (!includeDetalles)
                return new Carrito(e.id_carrito, e.id_usuario, e.fecha_creacion ?? DateTime.UtcNow);

            var detalles = new List<CarritoDetalle>();
            foreach (var d in e.carrito_detalles)
            {
                //  Ajusta esta línea si tu CarritoDetalle tiene otra firma
                detalles.Add(new CarritoDetalle(d.id_carrito, d.id_producto, d.cantidad, d.precio_unitario)
                );
            }

            return new Carrito(e.id_carrito, e.id_usuario, e.fecha_creacion ?? DateTime.UtcNow, detalles);
        }

        // Dominio -> Infra
        private static carrito ToEntity(Carrito d) => new carrito
        {
            id_carrito    = d.Id,         // 0 si es nuevo → identity en DB
            id_usuario    = d.UsuarioId,
            fecha_creacion = d.FechaCreacion // tu BD tiene default now(), pero enviarlo no molesta
        };

        // ---------- CRUD ----------
        public async Task<Carrito?> GetByIdAsync(int id)
        {
            var e = await _context.Set<carrito>()
                                  .Include(x => x.carrito_detalles)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.id_carrito == id);

            return e is null ? null : ToDomain(e, includeDetalles: true);
        }

        public async Task<Carrito?> GetActivoByUsuarioAsync(int usuarioId)
        {
            // Por tu índice único en id_usuario, debe existir 0-1 carrito por usuario
            var e = await _context.Set<carrito>()
                                  .Include(x => x.carrito_detalles)
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(x => x.id_usuario == usuarioId);

            return e is null ? null : ToDomain(e, includeDetalles: true);
        }

        public async Task AddAsync(Carrito c)
        {
            var e = ToEntity(c);
            await _context.Set<carrito>().AddAsync(e);
            await _context.SaveChangesAsync();

            // (Opcional) reflejar Id generado en dominio:
            // typeof(Carrito).GetProperty(nameof(Carrito.Id))!.SetValue(c, e.id_carrito);
        }

        public async Task UpdateAsync(Carrito c)
        {
            var e = ToEntity(c);
            _context.Set<carrito>().Attach(e);

            // Normalmente no cambias fecha_creacion; marca lo necesario
            _context.Entry(e).Property(x => x.id_usuario).IsModified = true;
            // _context.Entry(e).Property(x => x.fecha_creacion).IsModified = false;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var set = _context.Set<carrito>();
            var e = await set.FirstOrDefaultAsync(x => x.id_carrito == id);
            if (e is null) return;

            set.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
