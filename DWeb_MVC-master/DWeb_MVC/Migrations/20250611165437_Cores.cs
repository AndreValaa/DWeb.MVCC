using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWeb_MVC.Migrations
{
    /// <inheritdoc />
    public partial class Cores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoresProdutos",
                columns: table => new
                {
                    CoresId = table.Column<int>(type: "int", nullable: false),
                    ListaProdutosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoresProdutos", x => new { x.CoresId, x.ListaProdutosId });
                    table.ForeignKey(
                        name: "FK_CoresProdutos_Cores_CoresId",
                        column: x => x.CoresId,
                        principalTable: "Cores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoresProdutos_Produtos_ListaProdutosId",
                        column: x => x.ListaProdutosId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoresProdutos_ListaProdutosId",
                table: "CoresProdutos",
                column: "ListaProdutosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoresProdutos");

            migrationBuilder.DropTable(
                name: "Cores");
        }
    }
}
