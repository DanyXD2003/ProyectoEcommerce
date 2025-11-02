namespace Ecommerce.Application.DTOs
{
    public class MetodoPagoAdminDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? NumeroToken { get; set; }
        public string? Banco { get; set; }
        public DateOnly? FechaExpiracion { get; set; }

        // Información del usuario asociado
        public int UsuarioId { get; set; }
        public string UsuarioCorreo { get; set; } = string.Empty;
        public string UsuarioNombre { get; set; } = string.Empty;
    }
}
