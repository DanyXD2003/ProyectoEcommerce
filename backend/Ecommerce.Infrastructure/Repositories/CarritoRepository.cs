using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly EcommerceDbContext _context;
        public CarritoRepository(EcommerceDbContext context) => _context = context;

        private static Carrito ToDomain(Infrastructure.Entities.carrito e) =>
            new Carrito(e.id_carrito, e.id_usuario, e.activo);

        private static Infrastructure.Entities.carrito ToEntity(Carrito d) => new()
        {
            id = d.Id,
            usuario_id = d.UsuarioId,
            activo = d.Activo
        };

        public async Task<Carrito?> GetByIdAsync(int id)
        {
            var e = await _context.carritos.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task<Carrito?> GetActivoByUsuarioAsync(int usuarioId)
        {
            var e = await _context.carritos.AsNoTracking()
                .FirstOrDefaultAsync(x => x.usuario_id == usuarioId && x.activo);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Carrito c)
        {
            _context.carritos.Add(ToEntity(c));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Carrito c)
        {
            var e = ToEntity(c);
            _context.carritos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.carritos.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.carritos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
