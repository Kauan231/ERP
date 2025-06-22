using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class ShipmentClientRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_InventoryItems_inventoryItemId",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "clientId",
                table: "Shipments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_clientId",
                table: "Shipments",
                column: "clientId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_InventoryItems_inventoryItemId",
                table: "OrderItems",
                column: "inventoryItemId",
                principalTable: "InventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Clients_clientId",
                table: "Shipments",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_InventoryItems_inventoryItemId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Clients_clientId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_clientId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "clientId",
                table: "Shipments");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_InventoryItems_inventoryItemId",
                table: "OrderItems",
                column: "inventoryItemId",
                principalTable: "InventoryItems",
                principalColumn: "Id");
        }
    }
}
