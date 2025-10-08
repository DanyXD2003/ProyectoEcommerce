using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("producto")]
public partial class producto
{
    [Key]
    public int id_producto { get; set; }

    public int id_categoria { get; set; }

    [StringLength(255)]
    public string nombre { get; set; } = null!;

    public string? descripcion { get; set; }

    [Precision(12, 2)]
    public decimal precio { get; set; }

    public int? stock { get; set; }

    public bool? activo { get; set; }

    [InverseProperty("id_productoNavigation")]
    public virtual ICollection<carrito_detalle> carrito_detalles { get; set; } = new List<carrito_detalle>();

    [ForeignKey("id_categoria")]
    [InverseProperty("productos")]
    public virtual categorium id_categoriaNavigation { get; set; } = null!;

    [InverseProperty("id_productoNavigation")]
    public virtual ICollection<pedido_detalle> pedido_detalles { get; set; } = new List<pedido_detalle>();

    [InverseProperty("id_productoNavigation")]
    public virtual ICollection<producto_imagen> producto_imagens { get; set; } = new List<producto_imagen>();
}
