using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainSchdule.DAL.Migrations
{
    public partial class addCompanyPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermissionCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionCompanies_AppUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCompanies_OwnerId",
                table: "PermissionCompanies",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionCompanies");
        }
    }
}
