namespace Ecommerce.Application.DTOs
{
    public class UsuarioActualizarDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
