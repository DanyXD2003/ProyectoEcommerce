using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;  
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CategoriumRepository : ICategoriumRepository
    {
        private readonly EcommerceDbContext _context;
        public CategoriumRepository(EcommerceDbContext context) => _context = context;

        private static Categorium ToDomain(Infrastructure.Entities.categoria e) =>
            new Categorium(e.id, e.nombre, e.descripcion, e.fecha_creacion);

        private static Infrastructure.Entities.categoria ToEntity(Categorium d) => new()
        {
            id = d.Id,
            nombre = d.Nombre,
            descripcion = d.Descripcion,
            fecha_creacion = d.FechaCreacion
        };

        public async Task<Categorium?> GetByIdAsync(int id)
        {
            var e = await _context.categorias.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
            return e is null ? null : ToDomain(e);
        }

        public async Task AddAsync(Categorium categorium)
        {
            _context.categorias.Add(ToEntity(categorium));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Categorium categorium)
        {
            var e = ToEntity(categorium);
            _context.categorias.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.categorias.FirstOrDefaultAsync(x => x.id == id);
            if (e is null) return;
            _context.categorias.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}