namespace Ecommerce.Domain.Entities;

public class MetodoPago
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }

    public string Tipo { get; private set; }
    public string? NumeroToken { get; private set; }
    public string? Banco { get; private set; }
    public DateOnly? FechaExpiracion { get; private set; }

    // Relaciones
    public Usuario Usuario { get; private set; } = null!;
    public List<Pedido> Pedidos { get; private set; } = new();

    // Constructor
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

    // Métodos de dominio
    public void ActualizarDatos(string? numeroToken = null, string? banco = null, DateOnly? fechaExpiracion = null)
    {
        NumeroToken = numeroToken;
        Banco = banco;
        FechaExpiracion = fechaExpiracion;
    }

    public bool EstaVencido()
    {
        if (!FechaExpiracion.HasValue)
            return false;

        return FechaExpiracion.Value < DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public override string ToString()
    {
        string vencimiento = FechaExpiracion.HasValue ? FechaExpiracion.Value.ToString("yyyy-MM-dd") : "Sin fecha de expiración";
        return $"{Tipo} - Banco: {Banco ?? "N/A"} ({vencimiento})";
    }
}
