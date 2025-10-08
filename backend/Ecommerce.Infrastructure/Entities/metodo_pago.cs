using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("metodo_pago")]
public partial class metodo_pago
{
    [Key]
    public int id_metodo_pago { get; set; }

    public int id_usuario { get; set; }

    [StringLength(30)]
    public string tipo { get; set; } = null!;

    public string? numero_token { get; set; }

    [StringLength(100)]
    public string? banco { get; set; }

    public DateOnly? fecha_expiracion { get; set; }

    [ForeignKey("id_usuario")]
    [InverseProperty("metodo_pagos")]
    public virtual usuario id_usuarioNavigation { get; set; } = null!;

    [InverseProperty("id_metodo_pagoNavigation")]
    public virtual ICollection<pedido> pedidos { get; set; } = new List<pedido>();
}
