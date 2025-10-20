using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPC.Migrations
{
    /// <inheritdoc />
    public partial class AddProductUnitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductUnits",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    productId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    imei = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    serialNumber = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUnits", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductUnits_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnits_imei",
                table: "ProductUnits",
                column: "imei",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnits_productId",
                table: "ProductUnits",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnits_serialNumber",
                table: "ProductUnits",
                column: "serialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductUnits");
        }
    }
}
