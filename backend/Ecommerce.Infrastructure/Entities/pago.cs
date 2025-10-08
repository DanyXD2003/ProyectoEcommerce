using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("pago")]
public partial class pago
{
    [Key]
    public int id_pago { get; set; }

    public int id_pedido { get; set; }

    [Precision(12, 2)]
    public decimal monto { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? fecha_pago { get; set; }

    [StringLength(50)]
    public string? metodo { get; set; }

    [StringLength(30)]
    public string? estado_pago { get; set; }

    [ForeignKey("id_pedido")]
    [InverseProperty("pagos")]
    public virtual pedido id_pedidoNavigation { get; set; } = null!;
}
