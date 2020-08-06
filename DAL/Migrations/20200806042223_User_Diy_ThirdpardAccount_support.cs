using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class User_Diy_ThirdpardAccount_support : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThirdpardAccount_AppUserDiyInfos_UserDiyInfoId",
                table: "ThirdpardAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThirdpardAccount",
                table: "ThirdpardAccount");

            migrationBuilder.RenameTable(
                name: "ThirdpardAccount",
                newName: "ThirdpardAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_ThirdpardAccount_UserDiyInfoId",
                table: "ThirdpardAccounts",
                newName: "IX_ThirdpardAccounts_UserDiyInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThirdpardAccounts",
                table: "ThirdpardAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThirdpardAccounts_AppUserDiyInfos_UserDiyInfoId",
                table: "ThirdpardAccounts",
                column: "UserDiyInfoId",
                principalTable: "AppUserDiyInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThirdpardAccounts_AppUserDiyInfos_UserDiyInfoId",
                table: "ThirdpardAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThirdpardAccounts",
                table: "ThirdpardAccounts");

            migrationBuilder.RenameTable(
                name: "ThirdpardAccounts",
                newName: "ThirdpardAccount");

            migrationBuilder.RenameIndex(
                name: "IX_ThirdpardAccounts_UserDiyInfoId",
                table: "ThirdpardAccount",
                newName: "IX_ThirdpardAccount_UserDiyInfoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThirdpardAccount",
                table: "ThirdpardAccount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThirdpardAccount_AppUserDiyInfos_UserDiyInfoId",
                table: "ThirdpardAccount",
                column: "UserDiyInfoId",
                principalTable: "AppUserDiyInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
