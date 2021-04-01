using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AppMessage_Typi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAppMessageeInfos_AppUsers_UserId",
                table: "UserAppMessageeInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAppMessageeInfos",
                table: "UserAppMessageeInfos");

            migrationBuilder.RenameTable(
                name: "UserAppMessageeInfos",
                newName: "UserAppMessageInfos");

            migrationBuilder.RenameIndex(
                name: "IX_UserAppMessageeInfos_UserId",
                table: "UserAppMessageInfos",
                newName: "IX_UserAppMessageInfos_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAppMessageInfos",
                table: "UserAppMessageInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAppMessageInfos_AppUsers_UserId",
                table: "UserAppMessageInfos",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAppMessageInfos_AppUsers_UserId",
                table: "UserAppMessageInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAppMessageInfos",
                table: "UserAppMessageInfos");

            migrationBuilder.RenameTable(
                name: "UserAppMessageInfos",
                newName: "UserAppMessageeInfos");

            migrationBuilder.RenameIndex(
                name: "IX_UserAppMessageInfos_UserId",
                table: "UserAppMessageeInfos",
                newName: "IX_UserAppMessageeInfos_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAppMessageeInfos",
                table: "UserAppMessageeInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAppMessageeInfos_AppUsers_UserId",
                table: "UserAppMessageeInfos",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
