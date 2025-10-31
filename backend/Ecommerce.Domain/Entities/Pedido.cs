namespace Ecommerce.Domain.Entities;

public class Pedido
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public int? DireccionId { get; private set; }
    public int? MetodoPagoId { get; private set; }
    public DateTime FechaPedido { get; private set; }
    public decimal Total { get; private set; }

    public Usuario Usuario { get; private set; } = null!;
    public Direccion? Direccion { get; private set; }
    public MetodoPago? MetodoPago { get; private set; }
    public ICollection<PedidoDetalle> Detalles { get; private set; } = new List<PedidoDetalle>();
    public ICollection<Pago> Pagos { get; private set; } = new List<Pago>();
    public ICollection<Descuento> Descuentos { get; private set; } = new List<Descuento>();

    public Pedido(int usuarioId, int? direccionId = null, int? metodoPagoId = null)
    {
        UsuarioId = usuarioId;
        DireccionId = direccionId;
        MetodoPagoId = metodoPagoId;
        FechaPedido = DateTime.UtcNow;
        Total = 0;
    }

    public Pedido(int id, int usuarioId, int? direccionId, int? metodoPagoId, DateTime fechaPedido, decimal total)
        : this(usuarioId, direccionId, metodoPagoId)
    {
        Id = id;
        FechaPedido = fechaPedido;
        Total = total;
    }
}
