namespace Ecommerce.Application.DTOs
{
    public class UsuarioInfoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }

        public List<DireccionDTO> Direcciones { get; set; } = new();
        public List<PedidoResumenDTO> Pedidos { get; set; } = new();
    }

    public class DireccionDTO
    {
        public int Id { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }

    public class PedidoResumenDTO
    {
        public int Id { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public int DireccionId { get; set; }
        public MetodoPagoDTO MetodoPago { get; set; } = new();
    }

    public class MetodoPagoDTO
    {
        public string Tipo { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public DateTime FechaExpiracion { get; set; }
    }
}
