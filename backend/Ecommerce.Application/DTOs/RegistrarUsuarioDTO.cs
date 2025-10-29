namespace Ecommerce.Application.DTOs
{
    public class UsuarioRegistroDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string TipoCuenta { get; set; } = "Cliente"; // valor por defecto
    }
}
