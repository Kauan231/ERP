using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class removeInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Businesses_businessId",
                table: "Inventories");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Businesses_businessId",
                table: "Inventories",
                column: "businessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Businesses_businessId",
                table: "Inventories");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Businesses_businessId",
                table: "Inventories",
                column: "businessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
