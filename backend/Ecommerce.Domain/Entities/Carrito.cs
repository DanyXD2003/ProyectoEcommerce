namespace Ecommerce.Domain.Entities;

public class Carrito
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    // Relaciones (si las usas en Dominio)
    public Usuario? Usuario { get; private set; }
    public List<CarritoDetalle> Detalles { get; private set; } = new();

    // Crea un carrito nuevo (sin Id todavía)
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

    // Método de dominio: agregar un producto
    public void AgregarProducto(int productoId, int cantidad, decimal precioUnitario)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que 0.");

        var existente = Detalles.FirstOrDefault(d => d.ProductoId == productoId);

        if (existente is null)
        {
            //  Asegúrate que esta firma coincide con tu CarritoDetalle
            var nuevo = new CarritoDetalle(Id, productoId, cantidad, precioUnitario);
            Detalles.Add(nuevo);
        }
        else
        {
            existente.ActualizarCantidad(existente.Cantidad + cantidad);
        }
    }

    public void EliminarProducto(int productoId)
    {
        var d = Detalles.FirstOrDefault(x => x.ProductoId == productoId);
        if (d != null) Detalles.Remove(d);
    }

    public decimal CalcularTotal() => Detalles.Sum(d => d.CalcularSubtotal());
}
