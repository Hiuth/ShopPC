using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPC.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryIdFromBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brand_Category_categoryId",
                table: "Brand");

            migrationBuilder.DropIndex(
                name: "IX_Brand_categoryId",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Brand");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "categoryId",
                table: "Brand",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Brand_categoryId",
                table: "Brand",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brand_Category_categoryId",
                table: "Brand",
                column: "categoryId",
                principalTable: "Category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
