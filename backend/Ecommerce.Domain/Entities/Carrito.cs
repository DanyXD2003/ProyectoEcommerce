namespace Ecommerce.Domain.Entities
{
    public class Carrito
    {
        public int Id { get; private set; }
        public int UsuarioId { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public bool Activo { get; private set; } = true;

        // 🔹 Campos de descuento (nuevo)
        public int? DescuentoId { get; private set; }
        public decimal TotalDescuento { get; private set; } = 0;

        // 🔹 Totales
        public decimal TotalSinDescuento => Detalles.Sum(d => d.Subtotal);
        public decimal TotalConDescuento => Math.Max(TotalSinDescuento - TotalDescuento, 0);

        // Relaciones
        public Usuario? Usuario { get; private set; }
        public List<CarritoDetalle> Detalles { get; private set; } = new();
        public Descuento? Descuento { get; private set; }

        // Constructor
        public Carrito(int usuarioId)
        {
            UsuarioId = usuarioId;
            FechaCreacion = DateTime.UtcNow;
        }

        // Rehidratación
        public Carrito(int id, int usuarioId, DateTime fechaCreacion, bool activo = true, int? descuentoId = null, decimal totalDescuento = 0, List<CarritoDetalle>? detalles = null)
        {
            Id = id;
            UsuarioId = usuarioId;
            FechaCreacion = fechaCreacion;
            Activo = activo;
            DescuentoId = descuentoId;
            TotalDescuento = totalDescuento;

            if (detalles is not null)
                Detalles = detalles;
        }

        // 🔹 Métodos de dominio

        public void AplicarDescuento(Descuento descuento)
        {
            if (descuento == null)
                throw new ArgumentNullException(nameof(descuento));

            if (!descuento.Activo)
                throw new InvalidOperationException("El descuento no está activo.");

            Descuento = descuento;
            DescuentoId = descuento.Id;

            // 🧮 Calcular descuento basado en porcentaje
            var descuentoCalculado = TotalSinDescuento * (descuento.Porcentaje / 100);
            TotalDescuento = Math.Round(descuentoCalculado, 2);
        }

        public void QuitarDescuento()
        {
            Descuento = null;
            DescuentoId = null;
            TotalDescuento = 0;
        }

        public void Desactivar()
        {
            Activo = false;
        }
    }
}
