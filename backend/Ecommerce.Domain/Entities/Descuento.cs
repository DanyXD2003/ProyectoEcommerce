namespace Ecommerce.Domain.Entities
{
    public class Descuento
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;

        // 🔹 Porcentaje de descuento (0–100)
        public decimal Porcentaje { get; private set; }
        public bool Activo { get; private set; } = true;

        public List<Carrito> Carritos { get; private set; } = new();

        // Constructor principal
        public Descuento(string codigo, string descripcion, decimal porcentaje)
        {
            if (porcentaje <= 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje de descuento debe ser mayor a 0 y menor o igual a 100.");

            Codigo = codigo;
            Descripcion = descripcion;
            Porcentaje = porcentaje;
            Activo = true;
        }

        // Rehidratación desde persistencia
        public Descuento(int id, string codigo, string descripcion, decimal porcentaje, bool activo = true)
        {
            Id = id;
            Codigo = codigo;
            Descripcion = descripcion;
            Porcentaje = porcentaje;
            Activo = activo;
        }

        public void Desactivar() => Activo = false;
        public void Activar() => Activo = true;
    }
}
