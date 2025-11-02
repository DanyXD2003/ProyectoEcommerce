//Dto para crear un descuento
using System.ComponentModel.DataAnnotations;
namespace Ecommerce.Application.DTOs
{
    public class CrearDescuentoDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Porcentaje { get; set; }
    }

}