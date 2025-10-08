using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("pedido_detalle")]
public partial class pedido_detalle
{
    [Key]
    public int id_detalle { get; set; }

    public int id_pedido { get; set; }

    public int id_producto { get; set; }

    public int cantidad { get; set; }

    [Precision(12, 2)]
    public decimal precio_unitario { get; set; }

    [ForeignKey("id_pedido")]
    [InverseProperty("pedido_detalles")]
    public virtual pedido id_pedidoNavigation { get; set; } = null!;

    [ForeignKey("id_producto")]
    [InverseProperty("pedido_detalles")]
    public virtual producto id_productoNavigation { get; set; } = null!;
}
