namespace Ecommerce.Domain.Entities;

public class Producto
{
    // Propiedades con nombres del dominio (sin atributos EF)
    public int Id { get; private set; }
    public int CategoriaId { get; private set; }
    public string Nombre { get; private set; }
    public string? Descripcion { get; private set; }
    public decimal Precio { get; private set; }
    public int Stock { get; private set; }
    public bool Activo { get; private set; }

    // Relaciones del dominio
    public Categoria? Categoria { get; private set; }
    public List<CarritoDetalle> CarritoDetalles { get; private set; } = new();
    public List<PedidoDetalle> PedidoDetalles { get; private set; } = new();
    public List<ProductoImagen> Imagenes { get; private set; } = new();

    // Constructor (controla cómo se crea un producto)
    public Producto(int categoriaId, string nombre, decimal precio, int stock, bool activo, string? descripcion = null)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del producto no puede estar vacío.");

        if (precio <= 0)
            throw new ArgumentException("El precio debe ser mayor que 0.");

        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.");

        CategoriaId = categoriaId;
        Nombre = nombre;
        Precio = precio;
        Stock = stock;
        Activo = activo;
        Descripcion = descripcion;
    }

    // Método de dominio: cambiar el precio
    public void CambiarPrecio(decimal nuevoPrecio)
    {
        if (nuevoPrecio <= 0)
            throw new ArgumentException("El nuevo precio debe ser mayor que 0.");
        Precio = nuevoPrecio;
    }

    // Método de dominio: ajustar el stock
    public void AjustarStock(int cantidad)
    {
        int nuevoStock = Stock + cantidad;
        if (nuevoStock < 0)
            throw new InvalidOperationException("El stock no puede ser negativo.");
        Stock = nuevoStock;
    }

    // Método de dominio: activar/desactivar producto
    public void CambiarEstado(bool activo)
    {
        Activo = activo;
    }
}
