using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options) { }

        // ================================
        //           DBSETS
        // ================================
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

        // ================================
        //           MAPEOS
        // ================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- USUARIO ----------
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

            // ---------- CATEGORIA / PRODUCTO ----------
            modelBuilder.Entity<Categoria>().HasKey(c => c.Id);

            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Productos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p => p.CategoriaId);

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

            // ---------- PEDIDO / PEDIDO DETALLE ----------
            modelBuilder.Entity<Pedido>().HasKey(p => p.Id);

            // Pedido -> PedidoDetalle
            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Detalles)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.PedidoId);

            // Pedido -> Usuario
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);

            // Pedido -> Direccion (una dirección puede tener varios pedidos)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Direccion)
                .WithMany(d => d.Pedidos)
                .HasForeignKey(p => p.DireccionId)
                .IsRequired();


            // Pedido -> MetodoPago (opcional)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.MetodoPago)
                .WithMany(mp => mp.Pedidos)
                .HasForeignKey(p => p.MetodoPagoId)
                .IsRequired(false);

            // Pedido -> Carrito
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Carrito)
                .WithMany()
                .HasForeignKey(p => p.CarritoId);

            // PedidoDetalle
            modelBuilder.Entity<PedidoDetalle>().HasKey(pd => pd.Id);
            modelBuilder.Entity<PedidoDetalle>().Property(pd => pd.PrecioUnitario).HasPrecision(18, 2);

            // ---------- METODO DE PAGO ----------
            modelBuilder.Entity<MetodoPago>().HasKey(mp => mp.Id);

            // ---------- DIRECCION ----------
            modelBuilder.Entity<Direccion>().HasKey(d => d.Id);

            // ---------- DESCUENTO ----------
            modelBuilder.Entity<Descuento>().HasKey(d => d.Id);
            modelBuilder.Entity<Descuento>().Property(d => d.Porcentaje).HasPrecision(5, 2);

            // ---------- CARRITO ----------
            modelBuilder.Entity<Carrito>().HasKey(c => c.Id);

            modelBuilder.Entity<Carrito>()
                .HasMany(c => c.Detalles)
                .WithOne(cd => cd.Carrito)
                .HasForeignKey(cd => cd.CarritoId);

            // Carrito -> Descuento (opcional)
            modelBuilder.Entity<Carrito>()
                .HasOne(c => c.Descuento)
                .WithMany(d => d.Carritos)
                .HasForeignKey(c => c.DescuentoId)
                .IsRequired(false);

            modelBuilder.Entity<CarritoDetalle>().HasKey(cd => cd.Id);
            modelBuilder.Entity<CarritoDetalle>().Property(cd => cd.PrecioUnitario).HasPrecision(18, 2);
            modelBuilder.Entity<Carrito>().Property(c => c.TotalDescuento).HasPrecision(18, 2);

            // ---------- PAGO ----------
            modelBuilder.Entity<Pago>().HasKey(pg => pg.Id);
        }
    }
}
