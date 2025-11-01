// Ecommerce.Domain/Entities/Usuario.cs
namespace Ecommerce.Domain.Entities
{
    public class Usuario
    {
        // Propiedades del negocio
        public int Id { get; set; }
        public string Nombre { get; private set; }
        public string? Apellido { get; private set; }
        public string Correo { get; private set; }
        public string ContrasenaHash { get; private set; }
        public string Rol { get; private set; }
        public DateTime FechaRegistro { get; set; }
        public string? token_recuperacion { get; set; }
        public DateTime? fecha_expiracion_token { get; set; }

        // Relaciones (pueden representarse como colecciones del dominio)
        public ICollection<Direccion> Direcciones { get; private set; } = new List<Direccion>();
        public ICollection<MetodoPago> MetodosPago { get; private set; } = new List<MetodoPago>();
        public ICollection<Pedido> Pedidos { get; private set; } = new List<Pedido>();

        // Constructor principal
        public Usuario(string nombre, string apellido, string correo, string contrasenaHash, string rol)
        {
            //if (string.IsNullOrWhiteSpace(nombre))
            //throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

            if (string.IsNullOrWhiteSpace(correo))
                throw new ArgumentException("El correo no puede estar vacío.", nameof(correo));

            if (string.IsNullOrWhiteSpace(contrasenaHash))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(contrasenaHash));

            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            ContrasenaHash = contrasenaHash;
            Rol = rol;
            FechaRegistro = DateTime.UtcNow;
        }

        // Constructor usado para rehidratar desde la capa de persistencia
        public Usuario(int id, string nombre, string? apellido, string correo, string contrasenaHash, string? rol)
        {
            Id = id; // Si esto no se ejecuta correctamente, Id=0
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo ?? throw new ArgumentException("El correo no puede estar vacío.", nameof(correo));
            ContrasenaHash = contrasenaHash ?? throw new ArgumentException("La contraseña no puede estar vacía.", nameof(contrasenaHash));

            Rol = rol ?? string.Empty;
            FechaRegistro = DateTime.UtcNow;
        }


    }
}
