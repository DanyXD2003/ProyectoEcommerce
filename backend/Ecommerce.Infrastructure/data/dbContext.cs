using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options) { }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<Pedido> Pedidos { get; set; } = null!;
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; } = null!;
        public DbSet<Pago> Pagos { get; set; } = null!;
        public DbSet<MetodoPago> MetodosPago { get; set; } = null!;
        public DbSet<Direccion> Direcciones { get; set; } = null!;
        public DbSet<Descuento> Descuentos { get; set; } = null!;
        public DbSet<Carrito> Carritos { get; set; } = null!;
        public DbSet<CarritoDetalle> CarritoDetalles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Direcciones)
                .WithOne(d => d.Usuario)
                .HasForeignKey(d => d.UsuarioId);
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.MetodosPago)
                .WithOne(mp => mp.Usuario)
                .HasForeignKey(mp => mp.UsuarioId);
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Pedidos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId);

            // Categoria - Producto
            modelBuilder.Entity<Categoria>().HasKey(c => c.Id);
            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId);

            // Producto - PedidoDetalle / CarritoDetalle
            modelBuilder.Entity<Producto>().HasKey(p => p.Id);
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.PedidoDetalles)
                .WithOne(pd => pd.Producto)
                .HasForeignKey(pd => pd.ProductoId);
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.CarritoDetalles)
                .WithOne()
                .HasForeignKey(cd => cd.ProductoId)
                .IsRequired(false);

            // Pedido - PedidoDetalle / Pago / Descuento
            modelBuilder.Entity<Pedido>().HasKey(p => p.Id);
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Detalles)
                .WithOne(pd => pd.Pedido)
                .HasForeignKey(pd => pd.PedidoId);
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Pagos)
                .WithOne(pg => pg.Pedido)
                .HasForeignKey(pg => pg.PedidoId);
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Descuentos)
                .WithMany(d => d.Pedidos); // Relación muchos a muchos

            // PedidoDetalle
            modelBuilder.Entity<PedidoDetalle>().HasKey(pd => pd.Id);

            // Pago
            modelBuilder.Entity<Pago>().HasKey(pg => pg.Id);

            // MetodoPago
            modelBuilder.Entity<MetodoPago>().HasKey(mp => mp.Id);

            // Direccion
            modelBuilder.Entity<Direccion>().HasKey(d => d.Id);

            // Descuento
            modelBuilder.Entity<Descuento>().HasKey(d => d.Id);

            // Carrito
            modelBuilder.Entity<Carrito>().HasKey(c => c.Id);
            modelBuilder.Entity<Carrito>()
                .HasMany(c => c.Detalles)
                .WithOne(cd => cd.Carrito)
                .HasForeignKey(cd => cd.CarritoId);

            // CarritoDetalle
            modelBuilder.Entity<CarritoDetalle>().HasKey(cd => cd.Id);
        }
    }
}
