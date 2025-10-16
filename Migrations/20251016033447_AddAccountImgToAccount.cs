using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPC.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountImgToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "accountImg",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accountImg",
                table: "Users");
        }
    }
}
