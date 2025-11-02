// Ecommerce.Application/Descuentos/Dtos/DescuentoResponse.cs
namespace Ecommerce.Application.Descuentos.Dtos
{
    public class DescuentoDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Porcentaje { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activo { get; set; }

        // Útil para vistas/listados rápidos
        public bool EstaVigente =>
            Activo && DateTime.UtcNow >= FechaInicio && DateTime.UtcNow <= FechaFin;
    }
}
