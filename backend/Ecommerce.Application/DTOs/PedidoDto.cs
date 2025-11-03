namespace Ecommerce.Application.DTOs
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string TipoPago { get; set; } = string.Empty;
        public decimal Total { get; set; }

        public string Direccion { get; set; } = string.Empty; // Descripción legible
        public string? MetodoPago { get; set; }               // Ejemplo: "Tarjeta BAC (••••1234)"
    }
}
