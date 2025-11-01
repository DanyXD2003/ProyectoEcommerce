namespace Ecommerce.Application.DTOs
{
    public class ActualizarProductoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public bool Activo { get; set; }
    }
}
