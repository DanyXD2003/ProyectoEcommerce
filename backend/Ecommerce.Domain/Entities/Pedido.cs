namespace Ecommerce.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; private set; }
        public int UsuarioId { get; private set; }
        public int DireccionId { get; private set; }
        public int CarritoId { get; private set; }
        public int? MetodoPagoId { get; private set; } // null si pago contra entrega
        public DateTime FechaPedido { get; private set; } = DateTime.UtcNow;
        public decimal Total { get; private set; }
        public string TipoPago { get; private set; } = "ContraEntrega"; // Por defecto
        public string Estado { get; private set; } = "Pendiente";

        public Usuario Usuario { get; private set; } = null!;
        public Direccion Direccion { get; private set; } = null!;
        public MetodoPago? MetodoPago { get; private set; }
        public Carrito Carrito { get; private set; } = null!;
        public List<PedidoDetalle> Detalles { get; private set; } = new();

        // Constructor principal (checkout)
        public Pedido(
            int usuarioId,
            int direccionId,
            int carritoId,
            decimal total,
            string tipoPago,
            int? metodoPagoId = null)
        {
            if (total <= 0)
                throw new ArgumentException("El total del pedido debe ser mayor a 0.", nameof(total));

            UsuarioId = usuarioId;
            DireccionId = direccionId;
            CarritoId = carritoId;
            Total = total;
            TipoPago = tipoPago;
            MetodoPagoId = metodoPagoId;
            Estado = "Pendiente";
            FechaPedido = DateTime.UtcNow;
        }

        // Rehidratación desde persistencia
        public Pedido(int id, int usuarioId, int direccionId, int carritoId, decimal total, string tipoPago, string estado, int? metodoPagoId = null)
        {
            Id = id;
            UsuarioId = usuarioId;
            DireccionId = direccionId;
            CarritoId = carritoId;
            Total = total;
            TipoPago = tipoPago;
            Estado = estado;
            MetodoPagoId = metodoPagoId;
            FechaPedido = DateTime.UtcNow;
        } 

        public void ActualizarEstado(string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEstado))
                throw new ArgumentException("El estado no puede estar vacío.");

            var estadosValidos = new[] { "Pendiente", "EnProceso", "Enviado", "Entregado", "Cancelado" };
            if (!estadosValidos.Contains(nuevoEstado))
                throw new InvalidOperationException($"El estado '{nuevoEstado}' no es válido.");

            Estado = nuevoEstado;
        }

        public void MarcarComoPagado()
        {
            if (TipoPago != "ContraEntrega" && MetodoPagoId == null)
                throw new InvalidOperationException("No se puede marcar como pagado sin método de pago.");

            Estado = "EnProceso";
        }

        public void MarcarComoEntregado() => Estado = "Entregado";
        public void Cancelar() => Estado = "Cancelado";

        public void AsociarDetalles(List<PedidoDetalle> detalles)
        {
            if (detalles == null || !detalles.Any())
                throw new ArgumentException("El pedido debe tener al menos un detalle.");
            Detalles = detalles;
        }

        public void ActualizarTotal(decimal nuevoTotal)
        {
            if (nuevoTotal <= 0)
                throw new ArgumentException("El total del pedido debe ser mayor a 0.");
            Total = nuevoTotal;
        }
    }
}
