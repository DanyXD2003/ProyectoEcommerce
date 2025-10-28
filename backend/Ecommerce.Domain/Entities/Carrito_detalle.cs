namespace Ecommerce.Domain.Entities;

public class CarritoDetalle
{
    public int Id { get; private set; }
    public int CarritoId { get; private set; }
    public int ProductoId { get; private set; }
    public int Cantidad { get; private set; }
    public decimal PrecioUnitario { get; private set; }

    // Relaciones (solo referencias, sin anotaciones)
     public Carrito? Carrito { get; private set; }
    

    // Constructor para crear la entidad con validaciones
    public CarritoDetalle(int carritoId, int productoId, int cantidad, decimal precioUnitario)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.");
        if (precioUnitario <= 0)
            throw new ArgumentException("El precio unitario debe ser mayor a 0.");

        CarritoId = carritoId;
        ProductoId = productoId;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
    }

    // Método de dominio (ejemplo)
    public decimal CalcularSubtotal() => Cantidad * PrecioUnitario;

    // Método para actualizar cantidad
    public void ActualizarCantidad(int nuevaCantidad)
    {
        if (nuevaCantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.");
        Cantidad = nuevaCantidad;
    }
}
