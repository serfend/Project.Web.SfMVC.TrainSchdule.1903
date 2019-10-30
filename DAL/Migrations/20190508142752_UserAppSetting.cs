using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class UserAppSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationSettingId",
                table: "AppUserApplicationInfos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserApplicationSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LastSubmitApplyTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplicationSetting", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserApplicationInfos_ApplicationSettingId",
                table: "AppUserApplicationInfos",
                column: "ApplicationSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserApplicationInfos_UserApplicationSetting_ApplicationSettingId",
                table: "AppUserApplicationInfos",
                column: "ApplicationSettingId",
                principalTable: "UserApplicationSetting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserApplicationInfos_UserApplicationSetting_ApplicationSettingId",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropTable(
                name: "UserApplicationSetting");

            migrationBuilder.DropIndex(
                name: "IX_AppUserApplicationInfos_ApplicationSettingId",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropColumn(
                name: "ApplicationSettingId",
                table: "AppUserApplicationInfos");
        }
    }
}
