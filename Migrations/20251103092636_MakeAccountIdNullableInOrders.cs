using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPC.Migrations
{
    /// <inheritdoc />
    public partial class MakeAccountIdNullableInOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_accountId",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "accountId",
                table: "Order",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_accountId",
                table: "Order",
                column: "accountId",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_accountId",
                table: "Order");

            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "accountId",
                keyValue: null,
                column: "accountId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "accountId",
                table: "Order",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_accountId",
                table: "Order",
                column: "accountId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
