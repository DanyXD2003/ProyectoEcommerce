namespace Ecommerce.Domain.Entities;

public class Categoria
{
    public int Id { get; private set; }
    public string Nombre { get; private set; }
    public string? Descripcion { get; private set; }

    public List<Producto> Productos { get; private set; } = new();

    // Crea una nueva categoría (sin Id aún)
    public Categoria(string nombre, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la categoría no puede estar vacío.");
        Nombre = nombre;
        Descripcion = descripcion;
    }

    // 👇 NUEVO: rehidratación desde persistencia
    public Categoria(int id, string nombre, string? descripcion = null)
        : this(nombre, descripcion)
    {
        Id = id;
    }

    public void CambiarNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre de la categoría no puede estar vacío.");
        Nombre = nuevoNombre;
    }

    public void CambiarDescripcion(string? nuevaDescripcion) => Descripcion = nuevaDescripcion;

    public void AgregarProducto(Producto producto)
    {
        if (producto is null) throw new ArgumentNullException(nameof(producto));
        Productos.Add(producto);
    }

    public void EliminarProducto(int productoId)
    {
        var p = Productos.FirstOrDefault(x => x.Id == productoId);
        if (p != null) Productos.Remove(p);
    }
}
