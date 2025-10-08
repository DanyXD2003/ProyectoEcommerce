using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("pedido")]
public partial class pedido
{
    [Key]
    public int id_pedido { get; set; }

    public int id_usuario { get; set; }

    public int? id_direccion { get; set; }

    public int? id_metodo_pago { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? fecha_pedido { get; set; }

    [Precision(12, 2)]
    public decimal total { get; set; }

    [ForeignKey("id_direccion")]
    [InverseProperty("pedidos")]
    public virtual direccion? id_direccionNavigation { get; set; }

    [ForeignKey("id_metodo_pago")]
    [InverseProperty("pedidos")]
    public virtual metodo_pago? id_metodo_pagoNavigation { get; set; }

    [ForeignKey("id_usuario")]
    [InverseProperty("pedidos")]
    public virtual usuario id_usuarioNavigation { get; set; } = null!;

    [InverseProperty("id_pedidoNavigation")]
    public virtual ICollection<pago> pagos { get; set; } = new List<pago>();

    [InverseProperty("id_pedidoNavigation")]
    public virtual ICollection<pedido_detalle> pedido_detalles { get; set; } = new List<pedido_detalle>();

    [ForeignKey("id_pedido")]
    [InverseProperty("id_pedidos")]
    public virtual ICollection<descuento> id_descuentos { get; set; } = new List<descuento>();
}
