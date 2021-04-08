using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PermissionsFramePermissionItemRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionRoleRalatePermissions_PermissionItem_PermissionId",
                table: "PermissionRoleRalatePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_PermissionsUsers_PermissionItem_PermissionId",
                table: "PermissionsUsers");

            migrationBuilder.DropTable(
                name: "PermissionItem");

            migrationBuilder.DropIndex(
                name: "IX_PermissionsUsers_PermissionId",
                table: "PermissionsUsers");

            migrationBuilder.DropIndex(
                name: "IX_PermissionRoleRalatePermissions_PermissionId",
                table: "PermissionRoleRalatePermissions");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "PermissionsUsers");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "PermissionRoleRalatePermissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PermissionId",
                table: "PermissionsUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PermissionId",
                table: "PermissionRoleRalatePermissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PermissionItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionItem", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsUsers_PermissionId",
                table: "PermissionsUsers",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRoleRalatePermissions_PermissionId",
                table: "PermissionRoleRalatePermissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionRoleRalatePermissions_PermissionItem_PermissionId",
                table: "PermissionRoleRalatePermissions",
                column: "PermissionId",
                principalTable: "PermissionItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionsUsers_PermissionItem_PermissionId",
                table: "PermissionsUsers",
                column: "PermissionId",
                principalTable: "PermissionItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
