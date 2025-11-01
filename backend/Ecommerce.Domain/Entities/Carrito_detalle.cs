namespace Ecommerce.Domain.Entities;

public class CarritoDetalle
{
    public int Id { get; private set; }
    public int CarritoId { get; private set; }
    public int ProductoId { get; private set; }
    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;

    public Carrito? Carrito { get; private set; }

    public CarritoDetalle(int carritoId, int productoId, int cantidad, decimal precioUnitario)
    {
        if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a 0.");
        if (precioUnitario <= 0) throw new ArgumentException("El precio unitario debe ser mayor a 0.");

        CarritoId = carritoId;
        ProductoId = productoId;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
    }

    public CarritoDetalle(int id, int carritoId, int productoId, int cantidad, decimal precioUnitario)
        : this(carritoId, productoId, cantidad, precioUnitario)
    {
        Id = id;
    }
}
