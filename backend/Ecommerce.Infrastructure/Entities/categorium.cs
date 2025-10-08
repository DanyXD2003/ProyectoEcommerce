using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Entities;

public partial class categorium
{
    [Key]
    public int id_categoria { get; set; }

    [StringLength(100)]
    public string nombre { get; set; } = null!;

    public string? descripcion { get; set; }

    [InverseProperty("id_categoriaNavigation")]
    public virtual ICollection<producto> productos { get; set; } = new List<producto>();
}
