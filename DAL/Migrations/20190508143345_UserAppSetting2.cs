using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class UserAppSetting2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserApplicationInfos_UserApplicationSetting_ApplicationSettingId",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserApplicationSetting",
                table: "UserApplicationSetting");

            migrationBuilder.RenameTable(
                name: "UserApplicationSetting",
                newName: "AppUserApplicationSettings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserApplicationSettings",
                table: "AppUserApplicationSettings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserApplicationInfos_AppUserApplicationSettings_ApplicationSettingId",
                table: "AppUserApplicationInfos",
                column: "ApplicationSettingId",
                principalTable: "AppUserApplicationSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserApplicationInfos_AppUserApplicationSettings_ApplicationSettingId",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserApplicationSettings",
                table: "AppUserApplicationSettings");

            migrationBuilder.RenameTable(
                name: "AppUserApplicationSettings",
                newName: "UserApplicationSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserApplicationSetting",
                table: "UserApplicationSetting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserApplicationInfos_UserApplicationSetting_ApplicationSettingId",
                table: "AppUserApplicationInfos",
                column: "ApplicationSettingId",
                principalTable: "UserApplicationSetting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
