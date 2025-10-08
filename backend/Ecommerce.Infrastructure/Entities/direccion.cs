using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("direccion")]
public partial class direccion
{
    [Key]
    public int id_direccion { get; set; }

    public int id_usuario { get; set; }

    public string calle { get; set; } = null!;

    [StringLength(100)]
    public string ciudad { get; set; } = null!;

    [StringLength(100)]
    public string? departamento { get; set; }

    [StringLength(20)]
    public string? codigo_postal { get; set; }

    [StringLength(100)]
    public string? pais { get; set; }

    [ForeignKey("id_usuario")]
    [InverseProperty("direccions")]
    public virtual usuario id_usuarioNavigation { get; set; } = null!;

    [InverseProperty("id_direccionNavigation")]
    public virtual ICollection<pedido> pedidos { get; set; } = new List<pedido>();
}
