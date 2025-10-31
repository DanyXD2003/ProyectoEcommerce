namespace Ecommerce.Domain.Entities;

public class Descuento
{
    public int Id { get; private set; }
    public string Codigo { get; private set; }
    public string? Descripcion { get; private set; }
    public decimal Porcentaje { get; private set; }
    public DateTime FechaInicio { get; private set; }
    public DateTime FechaFin { get; private set; }
    public bool Activo { get; private set; }

    public List<Pedido> Pedidos { get; private set; } = new();

    public Descuento(string codigo, decimal porcentaje, DateTime fechaInicio, DateTime fechaFin, string? descripcion = null, bool activo = true)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new ArgumentException("El código del descuento no puede estar vacío.");
        if (porcentaje <= 0 || porcentaje > 100) throw new ArgumentException("El porcentaje debe estar entre 0 y 100.");
        if (fechaFin <= fechaInicio) throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio.");

        Codigo = codigo;
        Porcentaje = porcentaje;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        Descripcion = descripcion;
        Activo = activo;
    }

    public Descuento(int id, string codigo, string? descripcion, decimal porcentaje, DateTime fechaInicio, DateTime fechaFin, bool activo)
        : this(codigo, porcentaje, fechaInicio, fechaFin, descripcion, activo)
    {
        Id = id;
    }
}
