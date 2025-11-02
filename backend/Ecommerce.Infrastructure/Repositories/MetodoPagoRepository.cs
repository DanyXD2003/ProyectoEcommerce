using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly EcommerceDbContext _context;

        public MetodoPagoRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        // 🔹 Obtener todos los métodos de pago (admin)
        public async Task<IEnumerable<MetodoPago>> GetAllAsync()
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .Include(m => m.Usuario)
                .ToListAsync();
        }

        // 🔹 Obtener métodos de pago por usuario (usuario normal)
        public async Task<IEnumerable<MetodoPago>> GetByUsuarioAsync(int usuarioId)
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .Where(m => m.UsuarioId == usuarioId)
                .ToListAsync();
        }

        // 🔹 Obtener método de pago por Id
        public async Task<MetodoPago?> GetByIdAsync(int id)
        {
            return await _context.MetodosPago
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // 🔹 Agregar un nuevo método de pago
        public async Task AddAsync(MetodoPago metodo)
        {
            _context.MetodosPago.Add(metodo);
            await _context.SaveChangesAsync();
        }

        // 🔹 Actualizar un método de pago existente
        public async Task UpdateAsync(MetodoPago metodo)
        {
            _context.MetodosPago.Update(metodo);
            await _context.SaveChangesAsync();
        }

        // 🔹 Eliminar método de pago
        public async Task DeleteAsync(int id)
        {
            var metodo = await _context.MetodosPago.FindAsync(id);
            if (metodo == null)
                return;

            _context.MetodosPago.Remove(metodo);
            await _context.SaveChangesAsync();
        }
    }
}
