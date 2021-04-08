using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PermissionsFrame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserApplicationInfos_Permissions_PermissionId",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_AppUserApplicationInfos_PermissionId",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "AppUserApplicationInfos");

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "ApplyAuditStreams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "ApplyAuditStreamNodeActions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PermissionItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionsRoles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreteById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateById = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsRoles", x => x.Name);
                    table.ForeignKey(
                        name: "FK_PermissionsRoles_AppUsers_CreteById",
                        column: x => x.CreteById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionsUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionsUsers_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionsUsers_PermissionItem_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "PermissionItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRoleRalatePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSelf = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRoleRalatePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionRoleRalatePermissions_PermissionItem_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "PermissionItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionRoleRalatePermissions_PermissionsRoles_RoleName",
                        column: x => x.RoleName,
                        principalTable: "PermissionsRoles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionsRoleRelates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FromName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsRoleRelates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionsRoleRelates_PermissionsRoles_FromName",
                        column: x => x.FromName,
                        principalTable: "PermissionsRoles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionsRoleRelates_PermissionsRoles_ToName",
                        column: x => x.ToName,
                        principalTable: "PermissionsRoles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionsUserRelates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionsUserRelates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionsUserRelates_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionsUserRelates_PermissionsRoles_RoleName",
                        column: x => x.RoleName,
                        principalTable: "PermissionsRoles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRoleRalatePermissions_PermissionId",
                table: "PermissionRoleRalatePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRoleRalatePermissions_RoleName",
                table: "PermissionRoleRalatePermissions",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsRoleRelates_FromName",
                table: "PermissionsRoleRelates",
                column: "FromName");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsRoleRelates_ToName",
                table: "PermissionsRoleRelates",
                column: "ToName");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsRoles_CreteById",
                table: "PermissionsRoles",
                column: "CreteById");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsUserRelates_RoleName",
                table: "PermissionsUserRelates",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsUserRelates_UserId",
                table: "PermissionsUserRelates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsUsers_PermissionId",
                table: "PermissionsUsers",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionsUsers_UserId",
                table: "PermissionsUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionRoleRalatePermissions");

            migrationBuilder.DropTable(
                name: "PermissionsRoleRelates");

            migrationBuilder.DropTable(
                name: "PermissionsUserRelates");

            migrationBuilder.DropTable(
                name: "PermissionsUsers");

            migrationBuilder.DropTable(
                name: "PermissionsRoles");

            migrationBuilder.DropTable(
                name: "PermissionItem");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "ApplyAuditStreams");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.AddColumn<Guid>(
                name: "PermissionId",
                table: "AppUserApplicationInfos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Regions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserApplicationInfos_PermissionId",
                table: "AppUserApplicationInfos",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserApplicationInfos_Permissions_PermissionId",
                table: "AppUserApplicationInfos",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
