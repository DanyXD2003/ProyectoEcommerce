using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly EcommerceDbContext _context;
        public MetodoPagoRepository(EcommerceDbContext context) => _context = context;

       // Convierte de entidad de infraestructura (EF) → dominio (lógica de negocio)
private static MetodoPago ToDomain(MetodoPago e)
{
    // Crea un nuevo objeto del dominio usando el constructor que ya tienes
    var metodo = new MetodoPago(
        e.Id,
        e.Tipo,
        e.NumeroToken,
        e.Banco,
        e.FechaExpiracion
    );

    // Asigna el Id (ya que no lo recibe en el constructor)
    typeof(MetodoPago)
        .GetProperty(nameof(MetodoPago.Id))!
        .SetValue(metodo, e.Id);

    return metodo;
}

        public async Task<IReadOnlyList<MetodoPago>> ListActivosAsync()
        {
            var entidades = await _context.MetodosPago
                .AsNoTracking()
                .ToListAsync();
            return entidades.Select(ToDomain).ToList();
        }

        public async Task AddAsync(MetodoPago m)
        {
            _context.MetodosPago.Add(ToDomain(m));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MetodoPago m)
        {
            var e = ToDomain(m);
            _context.MetodosPago.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.MetodosPago.FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return;
            _context.MetodosPago.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
