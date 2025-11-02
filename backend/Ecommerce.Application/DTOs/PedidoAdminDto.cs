namespace Ecommerce.Application.DTOs
{
    public class PedidoAdminDto
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string TipoPago { get; set; } = string.Empty;
        public decimal Total { get; set; }

        // Información del cliente
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string UsuarioCorreo { get; set; } = string.Empty;

        // Dirección resumida
        public string DireccionEnvio { get; set; } = string.Empty;

        // Método de pago
        public string? MetodoPago { get; set; } // Ejemplo: "Tarjeta BAC (••••1234)"
    }
}
