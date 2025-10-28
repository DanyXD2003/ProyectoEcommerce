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
private static MetodoPago ToDomain(Ecommerce.Infrastructure.Entities.metodo_pago e)
{
    // Crea un nuevo objeto del dominio usando el constructor que ya tienes
    var metodo = new MetodoPago(
        e.id_usuario,
        e.tipo,
        e.numero_token,
        e.banco,
        e.fecha_expiracion
    );

    // Asigna el Id (ya que no lo recibe en el constructor)
    typeof(MetodoPago)
        .GetProperty(nameof(MetodoPago.Id))!
        .SetValue(metodo, e.id_metodo_pago);

    return metodo;
}

// Convierte de dominio → entidad de infraestructura (para guardar en DB)
private static Ecommerce.Infrastructure.Entities.metodo_pago ToEntity(MetodoPago d) => new()
{
    id_metodo_pago = d.Id,             // Id del dominio
    id_usuario = d.UsuarioId,          // Clave foránea
    tipo = d.Tipo,
    numero_token = d.NumeroToken,
    banco = d.Banco,
    fecha_expiracion = d.FechaExpiracion
};

        public async Task<IReadOnlyList<MetodoPago>> ListActivosAsync()
        {
            var list = await _context.metodo_pagos
                .AsNoTracking()
                .OrderBy(x => x.id_metodo_pago)
                .OrderBy(x => x.tipo)
                .ToListAsync();

            return list.Select(ToDomain).ToList();
        }

        public async Task AddAsync(MetodoPago m)
        {
            _context.metodo_pagos.Add(ToEntity(m));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MetodoPago m)
        {
            var e = ToEntity(m);
            _context.metodo_pagos.Attach(e);
            _context.Entry(e).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.metodo_pagos.FirstOrDefaultAsync(x => x.id_metodo_pago == id);
            if (e is null) return;
            _context.metodo_pagos.Remove(e);
            await _context.SaveChangesAsync();
        }
    }
}
