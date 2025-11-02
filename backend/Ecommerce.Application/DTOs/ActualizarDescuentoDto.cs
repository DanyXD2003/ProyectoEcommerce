// Actualizar descuento Dto
// Ecommerce.Application/Descuentos/Dtos/DescuentoUpdateRequest.cs
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.Descuentos.Dtos
{
    public class ActualizarDescuentaDto
    {
        [Required]
        public int Id { get; set; }

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

        public bool Activo { get; set; }
    }
}
