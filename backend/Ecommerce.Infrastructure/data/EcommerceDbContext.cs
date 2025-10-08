using System;
using System.Collections.Generic;
using Ecommerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Data;

    public partial class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options)
    {
    }

    public virtual DbSet<carrito> carritos { get; set; }

    public virtual DbSet<carrito_detalle> carrito_detalles { get; set; }

    public virtual DbSet<categorium> categoria { get; set; }

    public virtual DbSet<descuento> descuentos { get; set; }

    public virtual DbSet<direccion> direccions { get; set; }

    public virtual DbSet<metodo_pago> metodo_pagos { get; set; }

    public virtual DbSet<pago> pagos { get; set; }

    public virtual DbSet<pedido> pedidos { get; set; }

    public virtual DbSet<pedido_detalle> pedido_detalles { get; set; }

    public virtual DbSet<producto> productos { get; set; }

    public virtual DbSet<producto_imagen> producto_imagens { get; set; }

    public virtual DbSet<usuario> usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum("estado_pedido", new[] { "pendiente", "procesando", "enviado", "entregado", "cancelado" });

        modelBuilder.Entity<carrito>(entity =>
        {
            entity.HasKey(e => e.id_carrito).HasName("carrito_pkey");

            entity.Property(e => e.id_carrito).UseIdentityAlwaysColumn();
            entity.Property(e => e.fecha_creacion).HasDefaultValueSql("now()");

            entity.HasOne(d => d.id_usuarioNavigation).WithOne(p => p.carrito).HasConstraintName("carrito_id_usuario_fkey");
        });

        modelBuilder.Entity<carrito_detalle>(entity =>
        {
            entity.HasKey(e => e.id_detalle).HasName("carrito_detalle_pkey");

            entity.Property(e => e.id_detalle).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.id_carritoNavigation).WithMany(p => p.carrito_detalles).HasConstraintName("carrito_detalle_id_carrito_fkey");

            entity.HasOne(d => d.id_productoNavigation).WithMany(p => p.carrito_detalles).HasConstraintName("carrito_detalle_id_producto_fkey");
        });

        modelBuilder.Entity<categorium>(entity =>
        {
            entity.HasKey(e => e.id_categoria).HasName("categoria_pkey");

            entity.Property(e => e.id_categoria).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<descuento>(entity =>
        {
            entity.HasKey(e => e.id_descuento).HasName("descuento_pkey");

            entity.Property(e => e.id_descuento).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
        });

        modelBuilder.Entity<direccion>(entity =>
        {
            entity.HasKey(e => e.id_direccion).HasName("direccion_pkey");

            entity.Property(e => e.id_direccion).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.direccions).HasConstraintName("direccion_id_usuario_fkey");
        });

        modelBuilder.Entity<metodo_pago>(entity =>
        {
            entity.HasKey(e => e.id_metodo_pago).HasName("metodo_pago_pkey");

            entity.Property(e => e.id_metodo_pago).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.metodo_pagos).HasConstraintName("metodo_pago_id_usuario_fkey");
        });

        modelBuilder.Entity<pago>(entity =>
        {
            entity.HasKey(e => e.id_pago).HasName("pago_pkey");

            entity.Property(e => e.id_pago).UseIdentityAlwaysColumn();
            entity.Property(e => e.estado_pago).HasDefaultValueSql("'pendiente'::character varying");
            entity.Property(e => e.fecha_pago).HasDefaultValueSql("now()");

            entity.HasOne(d => d.id_pedidoNavigation).WithMany(p => p.pagos).HasConstraintName("pago_id_pedido_fkey");
        });

        modelBuilder.Entity<pedido>(entity =>
        {
            entity.HasKey(e => e.id_pedido).HasName("pedido_pkey");

            entity.Property(e => e.id_pedido).UseIdentityAlwaysColumn();
            entity.Property(e => e.fecha_pedido).HasDefaultValueSql("now()");

            entity.HasOne(d => d.id_direccionNavigation).WithMany(p => p.pedidos)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pedido_id_direccion_fkey");

            entity.HasOne(d => d.id_metodo_pagoNavigation).WithMany(p => p.pedidos)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("pedido_id_metodo_pago_fkey");

            entity.HasOne(d => d.id_usuarioNavigation).WithMany(p => p.pedidos).HasConstraintName("pedido_id_usuario_fkey");

            entity.HasMany(d => d.id_descuentos).WithMany(p => p.id_pedidos)
                .UsingEntity<Dictionary<string, object>>(
                    "pedido_descuento",
                    r => r.HasOne<descuento>().WithMany()
                        .HasForeignKey("id_descuento")
                        .HasConstraintName("pedido_descuento_id_descuento_fkey"),
                    l => l.HasOne<pedido>().WithMany()
                        .HasForeignKey("id_pedido")
                        .HasConstraintName("pedido_descuento_id_pedido_fkey"),
                    j =>
                    {
                        j.HasKey("id_pedido", "id_descuento").HasName("pedido_descuento_pkey");
                        j.ToTable("pedido_descuento");
                    });
        });

        modelBuilder.Entity<pedido_detalle>(entity =>
        {
            entity.HasKey(e => e.id_detalle).HasName("pedido_detalle_pkey");

            entity.Property(e => e.id_detalle).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.id_pedidoNavigation).WithMany(p => p.pedido_detalles).HasConstraintName("pedido_detalle_id_pedido_fkey");

            entity.HasOne(d => d.id_productoNavigation).WithMany(p => p.pedido_detalles).HasConstraintName("pedido_detalle_id_producto_fkey");
        });

        modelBuilder.Entity<producto>(entity =>
        {
            entity.HasKey(e => e.id_producto).HasName("producto_pkey");

            entity.Property(e => e.id_producto).UseIdentityAlwaysColumn();
            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.stock).HasDefaultValue(0);

            entity.HasOne(d => d.id_categoriaNavigation).WithMany(p => p.productos).HasConstraintName("producto_id_categoria_fkey");
        });

        modelBuilder.Entity<producto_imagen>(entity =>
        {
            entity.HasKey(e => e.id_imagen).HasName("producto_imagen_pkey");

            entity.Property(e => e.id_imagen).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.id_productoNavigation).WithMany(p => p.producto_imagens).HasConstraintName("producto_imagen_id_producto_fkey");
        });

        modelBuilder.Entity<usuario>(entity =>
        {
            entity.HasKey(e => e.id_usuario).HasName("usuario_pkey");

            entity.Property(e => e.id_usuario).UseIdentityAlwaysColumn();
            entity.Property(e => e.fecha_registro).HasDefaultValueSql("now()");
            entity.Property(e => e.rol).HasDefaultValueSql("'cliente'::character varying");
        });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
