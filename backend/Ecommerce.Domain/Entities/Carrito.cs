namespace Ecommerce.Domain.Entities;

public class Carrito
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    // Relaciones (si las usas en Dominio)
    public Usuario? Usuario { get; private set; }
    public List<CarritoDetalle> Detalles { get; private set; } = new();

    // Constructor
    public Carrito(int usuarioId)
    {
        UsuarioId = usuarioId;
        FechaCreacion = DateTime.UtcNow;
    }

    // Rehidratación desde persistencia
    public Carrito(int id, int usuarioId, DateTime fechaCreacion, List<CarritoDetalle>? detalles = null)
    {
        Id = id;
        UsuarioId = usuarioId;
        FechaCreacion = fechaCreacion;
        if (detalles is not null) Detalles = detalles;
    }
}
