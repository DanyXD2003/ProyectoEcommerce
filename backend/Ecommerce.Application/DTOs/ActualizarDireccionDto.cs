namespace Ecommerce.Application.DTOs;
public class ActualizarDireccionDto
{
    public string Calle { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string? Departamento { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Pais { get; set; }
    public string? Telefono { get; set; }
}