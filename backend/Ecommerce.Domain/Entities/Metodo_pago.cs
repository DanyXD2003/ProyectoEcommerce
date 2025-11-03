namespace Ecommerce.Domain.Entities
{
    public class MetodoPago
    {
        public int Id { get; private set; }
        public int UsuarioId { get; private set; }
        public string Tipo { get; private set; }
        public string? NumeroToken { get; private set; }
        public string? Banco { get; private set; }
        public DateOnly? FechaExpiracion { get; private set; }

        public Usuario Usuario { get; private set; } = null!;
        public List<Pedido> Pedidos { get; private set; } = new();

        public MetodoPago(int usuarioId, string tipo, string? numeroToken = null, string? banco = null, DateOnly? fechaExpiracion = null)
        {
            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("El tipo de método de pago no puede estar vacío.", nameof(tipo));

            UsuarioId = usuarioId;
            Tipo = tipo;
            NumeroToken = numeroToken;
            Banco = banco;
            FechaExpiracion = fechaExpiracion;
        }

        public MetodoPago(int id, int usuarioId, string tipo, string? numeroToken, string? banco, DateOnly? fechaExpiracion)
            : this(usuarioId, tipo, numeroToken, banco, fechaExpiracion)
        {
            Id = id;
        }
    }
}
