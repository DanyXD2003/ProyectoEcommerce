namespace Ecommerce.Application.DTOs
{
    public class CrearPedidoDto
    {
        public int DireccionId { get; set; }
        public int? MetodoPagoId { get; set; }
        public string TipoPago { get; set; } = "ContraEntrega"; // Tarjeta / ContraEntrega
    }
}
