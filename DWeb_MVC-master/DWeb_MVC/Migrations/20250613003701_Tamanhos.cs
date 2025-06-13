using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWeb_MVC.Migrations
{
    /// <inheritdoc />
    public partial class Tamanhos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tamanhos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tamanhos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosTamanhos",
                columns: table => new
                {
                    ListaProdutosId = table.Column<int>(type: "int", nullable: false),
                    TamanhosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosTamanhos", x => new { x.ListaProdutosId, x.TamanhosId });
                    table.ForeignKey(
                        name: "FK_ProdutosTamanhos_Produtos_ListaProdutosId",
                        column: x => x.ListaProdutosId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosTamanhos_Tamanhos_TamanhosId",
                        column: x => x.TamanhosId,
                        principalTable: "Tamanhos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosTamanhos_TamanhosId",
                table: "ProdutosTamanhos",
                column: "TamanhosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutosTamanhos");

            migrationBuilder.DropTable(
                name: "Tamanhos");
        }
    }
}
