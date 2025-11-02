namespace Ecommerce.Application.DTOs
{
    public class UsuarioRegistroAdminDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string Rol { get; set; } = "cliente";
    }
}
