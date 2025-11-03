
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Application.DTOs
{
       public class ActualizarDescuentoDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Porcentaje { get; set; }
        public bool Activo { get; set; }
    }
}
