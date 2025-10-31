using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly EcommerceDbContext _context;
        public CategoriaRepository(EcommerceDbContext context) => _context = context;

        // Infra -> Dominio
        private static Categoria ToDomain(Categoria e)
            => new Categoria(e.Id, e.Nombre, e.Descripcion);

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            var set = _context.Set<Categoria>();
            var e =  await set.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e is null ? null : ToDomain(new Categoria(e.Id,  e.Nombre, e.Descripcion));
        }

        public async Task AddAsync(Categoria categoria)
        {
            var set = _context.Set<Categoria>();
            await set.AddAsync(categoria);
            await  _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Categoria categoria)
        {
            var set = _context.Set<Categoria>();
            var e = await set.FirstOrDefaultAsync(x => x.Id == categoria.Id);
            if (e is null) throw new InvalidOperationException("Categoría no encontrada");

            e.Nombre = categoria.Nombre;
            e.Descripcion = categoria.Descripcion;

            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var set = _context.Set<Categoria>();
            var e = await set.FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) throw new InvalidOperationException("Categoría no encontrada");

            set.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
