namespace Ecommerce.Application.DTOs { }

public class CrearPedidoDTO
{
    public int UsuarioId { get; set; }
    public int? DireccionId { get; set; }
    public int? MetodoPagoId { get; set; }
}
