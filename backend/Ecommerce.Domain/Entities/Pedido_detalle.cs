namespace Ecommerce.Domain.Entities;

public class PedidoDetalle
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public int ProductoId { get; private set; }
    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }

    public Pedido Pedido { get; private set; } = null!;
    public Producto Producto { get; private set; } = null!;

    // Constructor principal
    public PedidoDetalle(int productoId, int cantidad, decimal precioUnitario)
    {
        if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor que cero.", nameof(cantidad));
        if (precioUnitario <= 0) throw new ArgumentException("El precio unitario debe ser mayor que cero.", nameof(precioUnitario));

        ProductoId = productoId;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
    }

    // Constructor de rehidratación
    public PedidoDetalle(int id, int pedidoId, int productoId, int cantidad, decimal precioUnitario)
        : this(productoId, cantidad, precioUnitario)
    {
        Id = id;
        PedidoId = pedidoId;
    }
}
