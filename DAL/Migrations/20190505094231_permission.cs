using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class permission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Apply_ApplyId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_Company_CompanyId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_User_UserId",
                table: "Permissions");

            migrationBuilder.DropTable(
                name: "Apply");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "PermittingAuth");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PermissionRange");

            migrationBuilder.DropTable(
                name: "PermittingAction");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_ApplyId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_CompanyId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ApplyId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Permissions");

            migrationBuilder.AddColumn<string>(
                name: "Regions",
                table: "Permissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Regions",
                table: "Permissions");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyId",
                table: "Permissions",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Permissions",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Permissions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PermittingAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermittingAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRange",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateId = table.Column<Guid>(nullable: true),
                    ModifyId = table.Column<Guid>(nullable: true),
                    QueryId = table.Column<Guid>(nullable: true),
                    RemoveId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionRange_PermittingAction_CreateId",
                        column: x => x.CreateId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionRange_PermittingAction_ModifyId",
                        column: x => x.ModifyId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionRange_PermittingAction_QueryId",
                        column: x => x.QueryId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionRange_PermittingAction_RemoveId",
                        column: x => x.RemoveId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermittingAuth",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthBy = table.Column<Guid>(nullable: false),
                    Create = table.Column<DateTime>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    PermittingActionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermittingAuth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermittingAuth_PermittingAction_PermittingActionId",
                        column: x => x.PermittingActionId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Apply",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    审批流Id = table.Column<Guid>(nullable: true),
                    申请信息Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apply_PermissionRange_审批流Id",
                        column: x => x.审批流Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Apply_PermissionRange_申请信息Id",
                        column: x => x.申请信息Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    单位信息Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_PermissionRange_单位信息Id",
                        column: x => x.单位信息Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    基本信息Id = table.Column<Guid>(nullable: true),
                    社会关系Id = table.Column<Guid>(nullable: true),
                    职务信息Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_PermissionRange_基本信息Id",
                        column: x => x.基本信息Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_PermissionRange_社会关系Id",
                        column: x => x.社会关系Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_PermissionRange_职务信息Id",
                        column: x => x.职务信息Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ApplyId",
                table: "Permissions",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CompanyId",
                table: "Permissions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Apply_审批流Id",
                table: "Apply",
                column: "审批流Id");

            migrationBuilder.CreateIndex(
                name: "IX_Apply_申请信息Id",
                table: "Apply",
                column: "申请信息Id");

            migrationBuilder.CreateIndex(
                name: "IX_Company_单位信息Id",
                table: "Company",
                column: "单位信息Id");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRange_CreateId",
                table: "PermissionRange",
                column: "CreateId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRange_ModifyId",
                table: "PermissionRange",
                column: "ModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRange_QueryId",
                table: "PermissionRange",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRange_RemoveId",
                table: "PermissionRange",
                column: "RemoveId");

            migrationBuilder.CreateIndex(
                name: "IX_PermittingAuth_PermittingActionId",
                table: "PermittingAuth",
                column: "PermittingActionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_基本信息Id",
                table: "User",
                column: "基本信息Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_社会关系Id",
                table: "User",
                column: "社会关系Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_职务信息Id",
                table: "User",
                column: "职务信息Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Apply_ApplyId",
                table: "Permissions",
                column: "ApplyId",
                principalTable: "Apply",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_Company_CompanyId",
                table: "Permissions",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_User_UserId",
                table: "Permissions",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
