namespace Ecommerce.Domain.Entities;

public class Direccion
{
    public int Id { get; private set; }
    public int UsuarioId { get; private set; }
    public string Calle { get; private set; }
    public string Ciudad { get; private set; }
    public string? Departamento { get; private set; }
    public string? CodigoPostal { get; private set; }
    public string? Pais { get; private set; }
    public string? Telefono { get; private set; }

    public Usuario Usuario { get; private set; } = null!;
    public List<Pedido> Pedidos { get; private set; } = new();

    public Direccion(int usuarioId, string calle, string ciudad, string? departamento = null, string? codigoPostal = null, string? pais = null, string? telefono = null)
    {
        if (string.IsNullOrWhiteSpace(calle)) throw new ArgumentException("La calle no puede estar vacía.", nameof(calle));
        if (string.IsNullOrWhiteSpace(ciudad)) throw new ArgumentException("La ciudad no puede estar vacía.", nameof(ciudad));

        UsuarioId = usuarioId;
        Calle = calle;
        Ciudad = ciudad;
        Departamento = departamento;
        CodigoPostal = codigoPostal;
        Pais = pais;
        Telefono = telefono;
    }

    public Direccion(int id, int usuarioId, string calle, string ciudad, string? departamento, string? codigoPostal, string? pais, string? telefono)
        : this(usuarioId, calle, ciudad, departamento, codigoPostal, pais, telefono)
    {
        Id = id;
    }
}
