namespace Ecommerce.Domain.Entities;

public class ProductoImagen
{
    public int Id { get; private set; }
    public int ProductoId { get; private set; }
    public string Url { get; private set; }

    // Relación con la entidad Producto (solo navegación, sin dependencias EF)
    public Producto Producto { get; private set; } = null!;

    // Constructor para creación
    public ProductoImagen(int productoId, string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("La URL de la imagen no puede estar vacía.", nameof(url));

        ProductoId = productoId;
        Url = url;
    }

    // Método para actualizar la URL si fuera necesario
    public void CambiarUrl(string nuevaUrl)
    {
        if (string.IsNullOrWhiteSpace(nuevaUrl))
            throw new ArgumentException("La nueva URL no puede estar vacía.", nameof(nuevaUrl));

        Url = nuevaUrl;
    }

    public override string ToString()
    {
        return $"Imagen #{Id} del producto {ProductoId}: {Url}";
    }
}
