namespace Ecommerce.Application.DTOs
{
    public class PedidoDetalleCompletoDto
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string TipoPago { get; set; } = string.Empty;
        public decimal Total { get; set; }

        public string DireccionEnvio { get; set; } = string.Empty;
        public string? MetodoPago { get; set; }

        public List<PedidoDetalleDto> Detalles { get; set; } = new();
    }
}
