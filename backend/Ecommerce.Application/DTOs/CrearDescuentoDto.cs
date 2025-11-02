//Dto para crear un descuento
using System.ComponentModel.DataAnnotations;
namespace Ecommerce.Application.DTOs
{
    public class CrearDescuentoDto
    {
        [Required, StringLength(50)]
        public string Codigo { get; set; } = null!;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Range(0.01, 100, ErrorMessage = "El porcentaje debe estar entre 0.01 y 100.")]
        public decimal Porcentaje { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        // Por defecto se crea activo
        public bool Activo { get; set; } = true;
    }

}