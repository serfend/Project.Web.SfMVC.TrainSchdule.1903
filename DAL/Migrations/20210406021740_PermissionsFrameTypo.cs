using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PermissionsFrameTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionsRoles_AppUsers_CreteById",
                table: "PermissionsRoles");

            migrationBuilder.DropIndex(
                name: "IX_PermissionsRoles_CreteById",
                table: "PermissionsRoles");

            migrationBuilder.DropColumn(
                name: "CreteById",
                table: "PermissionsRoles");

            migrationBuilder.AlterColumn<string>(
                name: "CreateById",
                table: "PermissionsRoles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsRoles_CreateById",
                table: "PermissionsRoles",
                column: "CreateById");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionsRoles_AppUsers_CreateById",
                table: "PermissionsRoles",
                column: "CreateById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionsRoles_AppUsers_CreateById",
                table: "PermissionsRoles");

            migrationBuilder.DropIndex(
                name: "IX_PermissionsRoles_CreateById",
                table: "PermissionsRoles");

            migrationBuilder.AlterColumn<string>(
                name: "CreateById",
                table: "PermissionsRoles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreteById",
                table: "PermissionsRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsRoles_CreteById",
                table: "PermissionsRoles",
                column: "CreteById");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionsRoles_AppUsers_CreteById",
                table: "PermissionsRoles",
                column: "CreteById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
