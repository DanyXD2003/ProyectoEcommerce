namespace Ecommerce.Domain.Entities;

public class Pago
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public decimal Monto { get; private set; }
    public DateTime FechaPago { get; private set; }
    public string Metodo { get; private set; }
    public string Estado { get; private set; }

    public Pedido Pedido { get; private set; } = null!;

    public Pago(int pedidoId, decimal monto, string metodo, string estado = "Pendiente")
    {
        if (monto <= 0) throw new ArgumentException("El monto del pago debe ser mayor que cero.", nameof(monto));
        if (string.IsNullOrWhiteSpace(metodo)) throw new ArgumentException("El método de pago es obligatorio.", nameof(metodo));

        PedidoId = pedidoId;
        Monto = monto;
        Metodo = metodo;
        Estado = estado;
        FechaPago = DateTime.UtcNow;
    }

    public Pago(int id, int pedidoId, decimal monto, string metodo, string estado, DateTime fechaPago)
        : this(pedidoId, monto, metodo, estado)
    {
        Id = id;
        FechaPago = fechaPago;
    }
}

