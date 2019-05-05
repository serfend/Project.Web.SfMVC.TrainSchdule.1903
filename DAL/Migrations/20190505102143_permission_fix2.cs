using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class permission_fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_AppUsers_OwnerId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_OwnerId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Permissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Permissions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_OwnerId",
                table: "Permissions",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_AppUsers_OwnerId",
                table: "Permissions",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
