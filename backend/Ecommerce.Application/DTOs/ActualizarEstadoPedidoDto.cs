namespace Ecommerce.Application.DTOs
{
    public class ActualizarEstadoPedidoDto
    {
        public int PedidoId { get; set; }
        public string NuevoEstado { get; set; } = string.Empty; // Ej: "EnProceso", "Enviado", "Entregado", "Cancelado"
    }
}
