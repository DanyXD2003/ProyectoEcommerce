using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("descuento")]
[Index("codigo", Name = "descuento_codigo_key", IsUnique = true)]
public partial class descuento
{
    [Key]
    public int id_descuento { get; set; }

    [StringLength(50)]
    public string codigo { get; set; } = null!;

    public string? descripcion { get; set; }

    [Precision(5, 2)]
    public decimal? porcentaje { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? fecha_inicio { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? fecha_fin { get; set; }

    public bool? activo { get; set; }

    [ForeignKey("id_descuento")]
    [InverseProperty("id_descuentos")]
    public virtual ICollection<pedido> id_pedidos { get; set; } = new List<pedido>();
}
