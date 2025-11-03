using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class DescuentoRepository : IDescuentoRepository
    {
        private readonly EcommerceDbContext _context;
        public DescuentoRepository(EcommerceDbContext context) => _context = context;

        private static Descuento ToDomain(Descuento e) =>
            new Descuento(e.Id, e.Codigo, e.Descripcion, e.Porcentaje, e.Activo);

        // 🔹 Obtener todos los descuentos
        public async Task<IEnumerable<Descuento>> GetAllAsync()
        {
            var entities = await _context.Descuentos.AsNoTracking().ToListAsync();
            return entities.Select(ToDomain);
        }

        // 🔹 Obtener descuento por ID
        public async Task<Descuento?> GetByIdAsync(int id)
        {
            var e = await _context.Descuentos.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            return e is null ? null : ToDomain(e);
        }

        // 🔹 Obtener descuento por código
        public async Task<Descuento?> GetByCodigoAsync(string codigo)
        {
            var e = await _context.Descuentos.AsNoTracking().FirstOrDefaultAsync(d => d.Codigo == codigo);
            return e is null ? null : ToDomain(e);
        }

        // 🔹 Agregar nuevo descuento
        public async Task AddAsync(Descuento d)
        {
            var entity = new Descuento(d.Codigo, d.Descripcion, d.Porcentaje);
            _context.Descuentos.Add(entity);
            await _context.SaveChangesAsync();
        }

        // 🔹 Actualizar descuento existente
        public async Task UpdateAsync(Descuento d)
        {
            var entity = await _context.Descuentos.FindAsync(d.Id)
                ?? throw new KeyNotFoundException("Descuento no encontrado.");

            // Actualizar valores
            _context.Entry(entity).CurrentValues.SetValues(new
            {
                Codigo = d.Codigo,
                Descripcion = d.Descripcion,
                Porcentaje = d.Porcentaje,
                Activo = d.Activo
            });

            await _context.SaveChangesAsync();
        }

        // 🔹 Eliminar descuento
        public async Task DeleteAsync(int id)
        {
            var descuento = await _context.Descuentos.FindAsync(id);
            if (descuento != null)
            {
                _context.Descuentos.Remove(descuento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
