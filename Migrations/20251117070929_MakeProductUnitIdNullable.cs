using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPC.Migrations
{
    /// <inheritdoc />
    public partial class MakeProductUnitIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarrantyRecords_ProductUnits_productUnitId",
                table: "WarrantyRecords");

            migrationBuilder.AlterColumn<string>(
                name: "productUnitId",
                table: "WarrantyRecords",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_WarrantyRecords_ProductUnits_productUnitId",
                table: "WarrantyRecords",
                column: "productUnitId",
                principalTable: "ProductUnits",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarrantyRecords_ProductUnits_productUnitId",
                table: "WarrantyRecords");

            migrationBuilder.UpdateData(
                table: "WarrantyRecords",
                keyColumn: "productUnitId",
                keyValue: null,
                column: "productUnitId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "productUnitId",
                table: "WarrantyRecords",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_WarrantyRecords_ProductUnits_productUnitId",
                table: "WarrantyRecords",
                column: "productUnitId",
                principalTable: "ProductUnits",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
