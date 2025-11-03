namespace Ecommerce.Application.DTOs
{
    public class MetodoPagoDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? NumeroToken { get; set; }
        public string? Banco { get; set; }
        public DateOnly? FechaExpiracion { get; set; }
    }
}
