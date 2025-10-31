namespace Ecommerce.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; private set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

        public List<Producto> Productos { get; set; } = new();

        // Constructor principal: para crear nuevas categorías desde la lógica de negocio
        public Categoria(string nombre, string? descripcion = null)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

            Nombre = nombre;
            Descripcion = descripcion;
        }

        // Constructor de rehidratación: para reconstruir la entidad desde la base de datos
        public Categoria(int id, string nombre, string? descripcion = null)
        {
            Id = id;
            Nombre = nombre ?? throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            Descripcion = descripcion;
        }
    }
}
