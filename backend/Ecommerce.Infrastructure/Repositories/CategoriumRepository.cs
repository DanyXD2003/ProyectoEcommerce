using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly EcommerceDbContext _context;
        public CategoriaRepository(EcommerceDbContext context) => _context = context;

        // Infra -> Dominio
        private static Categoria ToDomain(categorium e)
            => new Categoria(e.id_categoria, e.nombre, e.descripcion);

        // Dominio -> Infra
        private static categorium ToEntity(Categoria d)
            => new categorium
            {
                id_categoria = d.Id, // 0 si es nueva
                nombre = d.Nombre,
                descripcion = d.Descripcion
            };

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            var set = _context.Set<categorium>();
            var e = await set.AsNoTracking()
                             .FirstOrDefaultAsync(x => x.id_categoria == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Categoria categoria)
        {
            var set = _context.Set<categorium>();
            var e = ToEntity(categoria);
            await set.AddAsync(e);
            await _context.SaveChangesAsync();

            // Si necesitas reflejar el Id generado en la entidad de dominio:
            // typeof(Categoria).GetProperty(nameof(Categoria.Id))!.SetValue(categoria, e.id_categoria);
        }

        public async Task UpdateAsync(Categoria categoria)
        {
            var set = _context.Set<categorium>();
            var e = ToEntity(categoria);

            set.Attach(e);
            _context.Entry(e).Property(x => x.nombre).IsModified = true;
            _context.Entry(e).Property(x => x.descripcion).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var set = _context.Set<categorium>();
            var e = await set.FirstOrDefaultAsync(x => x.id_categoria == id);
            if (e is null) return;

            set.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
