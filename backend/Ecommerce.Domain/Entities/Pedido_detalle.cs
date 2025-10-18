namespace Ecommerce.Domain.Entities;

public class PedidoDetalle
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public int ProductoId { get; private set; }

    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }

    // Relaciones
    public Pedido Pedido { get; private set; } = null!;
    public Producto Producto { get; private set; } = null!;

    // Constructor
    public PedidoDetalle(int productoId, int cantidad, decimal precioUnitario)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.", nameof(cantidad));

        if (precioUnitario <= 0)
            throw new ArgumentException("El precio unitario debe ser mayor que cero.", nameof(precioUnitario));

        ProductoId = productoId;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
    }

    // Métodos de dominio
    public decimal CalcularSubtotal()
    {
        return Cantidad * PrecioUnitario;
    }

    public void ActualizarCantidad(int nuevaCantidad)
    {
        if (nuevaCantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.", nameof(nuevaCantidad));

        Cantidad = nuevaCantidad;
    }

    public void ActualizarPrecio(decimal nuevoPrecio)
    {
        if (nuevoPrecio <= 0)
            throw new ArgumentException("El precio debe ser mayor que cero.", nameof(nuevoPrecio));

        PrecioUnitario = nuevoPrecio;
    }

    public override string ToString()
    {
        return $"{Cantidad} x {Producto?.Nombre ?? "Producto"} = {CalcularSubtotal():C}";
    }
}
