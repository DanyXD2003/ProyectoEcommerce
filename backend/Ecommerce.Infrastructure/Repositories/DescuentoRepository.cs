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

        private static Descuento ToDomain(Infrastructure.Entities.descuento e) =>
            new Descuento(e.codigo, e.porcentaje ?? 0, e.fecha_inicio ?? DateTime.MinValue, e.fecha_fin ?? DateTime.MinValue, e.descripcion, e.activo ?? false)
            {
                // Asignar el Id privado mediante reflexión o un constructor adicional si es necesario
            };

        private static Infrastructure.Entities.descuento ToEntity(Descuento d) => new()
        {
            id_descuento = d.Id,
            codigo = d.Codigo,
            porcentaje = d.Porcentaje,
            fecha_inicio = d.FechaInicio,
            fecha_fin = d.FechaFin,
            activo = d.Activo
        };

        public async Task<Descuento?> GetByCodigoAsync(string codigo)
        {
            var e = await _context.descuentos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.codigo == codigo);

            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Descuento d)
        {
            _context.descuentos.Add(ToEntity(d));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Descuento d)
        {
            var e = ToEntity(d);
            _context.descuentos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.descuentos.FirstOrDefaultAsync(x => x.id_descuento == id);
            if (e is null) return;
            _context.descuentos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
