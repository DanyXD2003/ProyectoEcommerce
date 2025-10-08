using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("usuario")]
[Index("correo", Name = "usuario_correo_key", IsUnique = true)]
public partial class usuario
{
    [Key]
    public int id_usuario { get; set; }

    [StringLength(100)]
    public string nombre { get; set; } = null!;

    [StringLength(100)]
    public string? apellido { get; set; }

    [StringLength(255)]
    public string correo { get; set; } = null!;

    [StringLength(512)]
    public string contraseña_hash { get; set; } = null!;

    [StringLength(30)]
    public string? telefono { get; set; }

    [StringLength(20)]
    public string? rol { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? fecha_registro { get; set; }

    [InverseProperty("id_usuarioNavigation")]
    public virtual carrito? carrito { get; set; }

    [InverseProperty("id_usuarioNavigation")]
    public virtual ICollection<direccion> direccions { get; set; } = new List<direccion>();

    [InverseProperty("id_usuarioNavigation")]
    public virtual ICollection<metodo_pago> metodo_pagos { get; set; } = new List<metodo_pago>();

    [InverseProperty("id_usuarioNavigation")]
    public virtual ICollection<pedido> pedidos { get; set; } = new List<pedido>();
}
