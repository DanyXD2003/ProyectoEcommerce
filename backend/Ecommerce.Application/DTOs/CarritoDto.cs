namespace Ecommerce.Application.DTOs
{
    public class CarritoDto
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public List<CarritoDetalleDto> Detalles { get; set; } = new();
        public decimal Total => Detalles.Sum(d => d.Subtotal);
    }
}
