using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("carrito")]
[Index("id_usuario", Name = "carrito_id_usuario_key", IsUnique = true)]
public partial class carrito
{
    [Key]
    public int id_carrito { get; set; }

    public int id_usuario { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? fecha_creacion { get; set; }

    [InverseProperty("id_carritoNavigation")]
    public virtual ICollection<carrito_detalle> carrito_detalles { get; set; } = new List<carrito_detalle>();

    [ForeignKey("id_usuario")]
    [InverseProperty("carrito")]
    public virtual usuario id_usuarioNavigation { get; set; } = null!;
}
