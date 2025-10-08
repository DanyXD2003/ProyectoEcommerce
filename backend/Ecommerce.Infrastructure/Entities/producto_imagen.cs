using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

[Table("producto_imagen")]
public partial class producto_imagen
{
    [Key]
    public int id_imagen { get; set; }

    public int id_producto { get; set; }

    public string url_imagen { get; set; } = null!;

    [ForeignKey("id_producto")]
    [InverseProperty("producto_imagens")]
    public virtual producto id_productoNavigation { get; set; } = null!;
}
