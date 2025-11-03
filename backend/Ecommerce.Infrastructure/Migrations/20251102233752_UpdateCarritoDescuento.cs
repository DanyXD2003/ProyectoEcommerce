using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarritoDescuento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DescuentoPedido");

            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "Descuentos");

            migrationBuilder.DropColumn(
                name: "FechaInicio",
                table: "Descuentos");

            migrationBuilder.AlterColumn<decimal>(
                name: "Porcentaje",
                table: "Descuentos",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Descuentos",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "Descuentos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DescuentoId",
                table: "Carritos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDescuento",
                table: "Carritos",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioUnitario",
                table: "CarritoDetalles",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateIndex(
                name: "IX_Descuentos_PedidoId",
                table: "Descuentos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Carritos_DescuentoId",
                table: "Carritos",
                column: "DescuentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carritos_Descuentos_DescuentoId",
                table: "Carritos",
                column: "DescuentoId",
                principalTable: "Descuentos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Descuentos_Pedidos_PedidoId",
                table: "Descuentos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carritos_Descuentos_DescuentoId",
                table: "Carritos");

            migrationBuilder.DropForeignKey(
                name: "FK_Descuentos_Pedidos_PedidoId",
                table: "Descuentos");

            migrationBuilder.DropIndex(
                name: "IX_Descuentos_PedidoId",
                table: "Descuentos");

            migrationBuilder.DropIndex(
                name: "IX_Carritos_DescuentoId",
                table: "Carritos");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Descuentos");

            migrationBuilder.DropColumn(
                name: "DescuentoId",
                table: "Carritos");

            migrationBuilder.DropColumn(
                name: "TotalDescuento",
                table: "Carritos");

            migrationBuilder.AlterColumn<decimal>(
                name: "Porcentaje",
                table: "Descuentos",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Descuentos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaFin",
                table: "Descuentos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaInicio",
                table: "Descuentos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioUnitario",
                table: "CarritoDetalles",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.CreateTable(
                name: "DescuentoPedido",
                columns: table => new
                {
                    DescuentosId = table.Column<int>(type: "integer", nullable: false),
                    PedidosId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DescuentoPedido", x => new { x.DescuentosId, x.PedidosId });
                    table.ForeignKey(
                        name: "FK_DescuentoPedido_Descuentos_DescuentosId",
                        column: x => x.DescuentosId,
                        principalTable: "Descuentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DescuentoPedido_Pedidos_PedidosId",
                        column: x => x.PedidosId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DescuentoPedido_PedidosId",
                table: "DescuentoPedido",
                column: "PedidosId");
        }
    }
}
