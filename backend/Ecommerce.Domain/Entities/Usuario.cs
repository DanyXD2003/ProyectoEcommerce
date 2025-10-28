// Ecommerce.Domain/Entities/Usuario.cs
namespace Ecommerce.Domain.Entities
{
    public class Usuario
    {
        // Propiedades del negocio
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public string? Apellido { get; private set; }
        public string Correo { get; private set; }
        public string ContrasenaHash { get; private set; }
        public string? Telefono { get; private set; }
        public string Rol { get; private set; }
        public DateTime FechaRegistro { get; set; }

        /*
        // Relaciones (pueden representarse como colecciones del dominio)
        public ICollection<Direccion> Direcciones { get; private set; } = new List<Direccion>();
        public ICollection<MetodoPago> MetodosPago { get; private set; } = new List<MetodoPago>();
        public ICollection<Pedido> Pedidos { get; private set; } = new List<Pedido>();
        */
        // Constructor principal
        public Usuario(string nombre, string correo, string contrasenaHash, string rol)
        {
            //if (string.IsNullOrWhiteSpace(nombre))
            //throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

            if (string.IsNullOrWhiteSpace(correo))
                throw new ArgumentException("El correo no puede estar vacío.", nameof(correo));

            if (string.IsNullOrWhiteSpace(contrasenaHash))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(contrasenaHash));

            Nombre = nombre;
            Correo = correo;
            ContrasenaHash = contrasenaHash;
            Rol = rol;
            FechaRegistro = DateTime.UtcNow;
        }

        // Constructor usado para rehidratar desde la capa de persistencia
        public Usuario(int id, string nombre, string? apellido, string correo, string contrasenaHash, string? telefono, string? rol, DateTime? fechaRegistro)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo ?? throw new ArgumentException("El correo no puede estar vacío.", nameof(correo));
            ContrasenaHash = contrasenaHash ?? throw new ArgumentException("La contraseña no puede estar vacía.", nameof(contrasenaHash));
            Telefono = telefono;
            Rol = rol ?? string.Empty;
            FechaRegistro = fechaRegistro ?? DateTime.UtcNow;
        }

        /*
        // Métodos del dominio (reglas de negocio)
        public void CambiarContraseña(string nuevoHash)
        {
            if (string.IsNullOrWhiteSpace(nuevoHash))
                throw new ArgumentException("La nueva contraseña no puede estar vacía.");

            ContrasenaHash = nuevoHash;
        }

        public void ActualizarTelefono(string telefono)
        {
            if (telefono?.Length > 30)
                throw new ArgumentException("El teléfono no puede tener más de 30 caracteres.");

            Telefono = telefono;
        }

        public void ActualizarRol(string nuevoRol)
        {
            Rol = nuevoRol;
        }
        */
    }
}
