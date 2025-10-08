using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("carrito_detalle")]
public partial class carrito_detalle
{
    [Key]
    public int id_detalle { get; set; }

    public int id_carrito { get; set; }

    public int id_producto { get; set; }

    public int cantidad { get; set; }

    [Precision(12, 2)]
    public decimal precio_unitario { get; set; }

    [ForeignKey("id_carrito")]
    [InverseProperty("carrito_detalles")]
    public virtual carrito id_carritoNavigation { get; set; } = null!;

    [ForeignKey("id_producto")]
    [InverseProperty("carrito_detalles")]
    public virtual producto id_productoNavigation { get; set; } = null!;
}
