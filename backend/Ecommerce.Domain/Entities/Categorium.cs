namespace Ecommerce.Domain.Entities;

public class Categoria
{
    public int Id { get; private set; }
    public string Nombre { get; private set; }
    public string? Descripcion { get; private set; }

    // Relación con productos
    public List<Producto> Productos { get; private set; } = new();

    // Constructor
    public Categoria(string nombre, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la categoría no puede estar vacío.");

        Nombre = nombre;
        Descripcion = descripcion;
    }

    // Método para actualizar nombre
    public void CambiarNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre de la categoría no puede estar vacío.");
        Nombre = nuevoNombre;
    }

    // Método para actualizar descripción
    public void CambiarDescripcion(string? nuevaDescripcion)
    {
        Descripcion = nuevaDescripcion;
    }

    // Método de dominio: agregar producto
    public void AgregarProducto(Producto producto)
    {
        if (producto == null)
            throw new ArgumentNullException(nameof(producto));

        Productos.Add(producto);
    }

    // Método de dominio: eliminar producto
    public void EliminarProducto(int productoId)
    {
        var producto = Productos.FirstOrDefault(p => p.Id == productoId);
        if (producto != null)
            Productos.Remove(producto);
    }
}
