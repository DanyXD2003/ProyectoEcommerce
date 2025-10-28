namespace Ecommerce.Application.DTOs
{
    public class UsuarioRegistroDTO
    {
        public string nombre { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string contrasena { get; set; } = string.Empty;
        public string tipoCuenta { get; set; } = "Cliente"; // valor por defecto
        public string fecha_registro { get; set; } = string.Empty;
    }
}
