namespace Ecommerce.Domain.Entities;

public class Carrito
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public DateTime FechaCreacion { get; private set; }

    // Relaciones
    public Usuario? Usuario { get; private set; }
    public List<CarritoDetalle> Detalles { get; private set; } = new();

    // Constructor
    public Carrito(int usuarioId)
    {
        UsuarioId = usuarioId;
        FechaCreacion = DateTime.UtcNow;
        
    }

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

        var detalleExistente = Detalles.FirstOrDefault(d => d.ProductoId == productoId);
        if (detalleExistente is null)
        {
            var nuevoDetalle = new CarritoDetalle(UsuarioId, productoId, cantidad, precioUnitario);
            Detalles.Add(nuevoDetalle);
        }
        else
        {
            detalleExistente.ActualizarCantidad(detalleExistente.Cantidad + cantidad);
        }
    }

    // Método de dominio: eliminar producto
    public void EliminarProducto(int productoId)
    {
        var detalle = Detalles.FirstOrDefault(d => d.ProductoId == productoId);
        if (detalle != null)Detalles.Remove(detalle);
    }

    // Método de dominio: calcular total del carrito
    public decimal CalcularTotal() =>Detalles.Sum(d => d.CalcularSubtotal());
}
