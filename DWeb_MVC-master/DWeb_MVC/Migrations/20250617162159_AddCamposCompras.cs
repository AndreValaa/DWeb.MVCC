using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWeb_MVC.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposCompras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Clientes_ClienteId",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "Pago",
                table: "Compras");

            migrationBuilder.RenameColumn(
                name: "ClientesFK",
                table: "Compras",
                newName: "QuantidadeTotal");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Compras",
                newName: "ClientesId");

            migrationBuilder.RenameIndex(
                name: "IX_Compras_ClienteId",
                table: "Compras",
                newName: "IX_Compras_ClientesId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCompra",
                table: "Compras",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Compras",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecoTotal",
                table: "Compras",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProdutosComprados",
                table: "Compras",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Clientes_ClientesId",
                table: "Compras",
                column: "ClientesId",
                principalTable: "Clientes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Clientes_ClientesId",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "DataCompra",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "PrecoTotal",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "ProdutosComprados",
                table: "Compras");

            migrationBuilder.RenameColumn(
                name: "QuantidadeTotal",
                table: "Compras",
                newName: "ClientesFK");

            migrationBuilder.RenameColumn(
                name: "ClientesId",
                table: "Compras",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Compras_ClientesId",
                table: "Compras",
                newName: "IX_Compras_ClienteId");

            migrationBuilder.AddColumn<bool>(
                name: "Pago",
                table: "Compras",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Clientes_ClienteId",
                table: "Compras",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id");
        }
    }
}
