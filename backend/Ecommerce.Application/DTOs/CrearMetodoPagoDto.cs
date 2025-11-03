using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs
{
    public class CrearMetodoPagoDto
    {
        [Required]
        public string Tipo { get; set; } = string.Empty; // tarjeta, paypal, etc.

        [MaxLength(8)]
        public string? NumeroToken { get; set; } // últimos dígitos o referencia

        public string? Banco { get; set; }

        public DateOnly? FechaExpiracion { get; set; }
    }
}
