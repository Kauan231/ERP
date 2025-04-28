using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    public partial class business : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_AspNetUsers_userId",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Inventories",
                newName: "businessId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_userId",
                table: "Inventories",
                newName: "IX_Inventories_businessId");

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    userId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Businesses_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_userId",
                table: "Businesses",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Businesses_businessId",
                table: "Inventories",
                column: "businessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Businesses_businessId",
                table: "Inventories");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.RenameColumn(
                name: "businessId",
                table: "Inventories",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_businessId",
                table: "Inventories",
                newName: "IX_Inventories_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_AspNetUsers_userId",
                table: "Inventories",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
