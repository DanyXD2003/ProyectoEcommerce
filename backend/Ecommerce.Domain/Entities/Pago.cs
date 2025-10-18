namespace Ecommerce.Domain.Entities;

public class Pago
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }

    public decimal Monto { get; private set; }
    public DateTime FechaPago { get; private set; }
    public string Metodo { get; private set; }
    public string Estado { get; private set; }

    // Relación
    public Pedido Pedido { get; private set; } = null!;

    // Constructor
    public Pago(int pedidoId, decimal monto, string metodo, string estado = "Pendiente")
    {
        if (monto <= 0)
            throw new ArgumentException("El monto del pago debe ser mayor que cero.", nameof(monto));

        if (string.IsNullOrWhiteSpace(metodo))
            throw new ArgumentException("El método de pago es obligatorio.", nameof(metodo));

        PedidoId = pedidoId;
        Monto = monto;
        Metodo = metodo;
        Estado = estado;
        FechaPago = DateTime.UtcNow;
    }

    // Métodos de dominio

    /// <summary>
    /// Confirma que el pago fue recibido exitosamente.
    /// </summary>
    public void ConfirmarPago()
    {
        if (Estado == "Completado")
            throw new InvalidOperationException("El pago ya fue confirmado.");

        Estado = "Completado";
        FechaPago = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca el pago como fallido (por ejemplo, si el banco lo rechazó).
    /// </summary>
    public void MarcarComoFallido()
    {
        if (Estado == "Completado")
            throw new InvalidOperationException("No se puede marcar como fallido un pago completado.");

        Estado = "Fallido";
    }

    public override string ToString()
    {
        return $"Pago #{Id}: {Monto:C} - {Metodo} ({Estado}) en {FechaPago:yyyy-MM-dd HH:mm}";
    }
}
