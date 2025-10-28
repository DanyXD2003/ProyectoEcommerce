namespace Ecommerce.Domain.Entities;

public class Pedido
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public int? DireccionId { get; private set; }
    public int? MetodoPagoId { get; private set; }
    public DateTime FechaPedido { get; private set; }
    public decimal Total { get; private set; }

    // Relaciones de navegación
    public Usuario Usuario { get; private set; } = null!;
    public Direccion? Direccion { get; private set; }
    public MetodoPago? MetodoPago { get; private set; }
    public ICollection<PedidoDetalle> Detalles { get; private set; } = new List<PedidoDetalle>();
    public ICollection<Pago> Pagos { get; private set; } = new List<Pago>();
    public ICollection<Descuento> Descuentos { get; private set; } = new List<Descuento>();

    // Constructor
    public Pedido(int usuarioId, int? direccionId = null, int? metodoPagoId = null)
    {
        UsuarioId = usuarioId;
        DireccionId = direccionId;
        MetodoPagoId = metodoPagoId;
        FechaPedido = DateTime.UtcNow;
        Total = 0;
    }

    // Métodos de negocio

    public void AgregarDetalle(PedidoDetalle detalle)
    {
        if (detalle == null)
            throw new ArgumentNullException(nameof(detalle));

        Detalles.Add(detalle);
        CalcularTotal();
    }

    public void EliminarDetalle(PedidoDetalle detalle)
    {
        if (detalle == null)
            throw new ArgumentNullException(nameof(detalle));

        Detalles.Remove(detalle);
        CalcularTotal();
    }

    public void CalcularTotal()
    {
        Total = Detalles.Sum(d => d.CalcularSubtotal());
    }
public void AplicarDescuento(Descuento descuento)
{
    if (descuento == null)
        throw new ArgumentNullException(nameof(descuento));

    Descuentos.Add(descuento);

    // Calcula el nuevo total usando el método Aplicar
    Total = descuento.Aplicar(Total);

    if (Total < 0) Total = 0;
}


    public void RegistrarPago(Pago pago)
    {
        if (pago == null)
            throw new ArgumentNullException(nameof(pago));

        Pagos.Add(pago);
    }

    public override string ToString()
    {
        return $"Pedido #{Id} - Total: {Total:C} - Fecha: {FechaPedido:d}";
    }
}
