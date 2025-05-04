using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class inventoryIdShipments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "inventoryId",
                table: "Shipments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_inventoryId",
                table: "Shipments",
                column: "inventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Inventories_inventoryId",
                table: "Shipments",
                column: "inventoryId",
                principalTable: "Inventories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Inventories_inventoryId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_inventoryId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "inventoryId",
                table: "Shipments");
        }
    }
}
