using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs
{
    public class ActualizarMetodoPagoDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [MaxLength(8)]
        public string? NumeroToken { get; set; }

        public string? Banco { get; set; }

        public DateOnly? FechaExpiracion { get; set; }

        [Required]
        public int UsuarioId { get; set; }
    }
}
