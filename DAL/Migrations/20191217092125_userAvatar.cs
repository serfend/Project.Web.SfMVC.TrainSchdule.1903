using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class userAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_UserDiyInfo_DiyInfoId",
                table: "AppUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDiyInfo",
                table: "UserDiyInfo");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "UserDiyInfo");

            migrationBuilder.RenameTable(
                name: "UserDiyInfo",
                newName: "AppUserDiyInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarId",
                table: "AppUserDiyInfos",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserDiyInfos",
                table: "AppUserDiyInfos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppUserDiyAvatars",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FilePath = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserDiyAvatars", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserDiyInfos_AvatarId",
                table: "AppUserDiyInfos",
                column: "AvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserDiyInfos_AppUserDiyAvatars_AvatarId",
                table: "AppUserDiyInfos",
                column: "AvatarId",
                principalTable: "AppUserDiyAvatars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AppUserDiyInfos_DiyInfoId",
                table: "AppUsers",
                column: "DiyInfoId",
                principalTable: "AppUserDiyInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserDiyInfos_AppUserDiyAvatars_AvatarId",
                table: "AppUserDiyInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AppUserDiyInfos_DiyInfoId",
                table: "AppUsers");

            migrationBuilder.DropTable(
                name: "AppUserDiyAvatars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserDiyInfos",
                table: "AppUserDiyInfos");

            migrationBuilder.DropIndex(
                name: "IX_AppUserDiyInfos_AvatarId",
                table: "AppUserDiyInfos");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "AppUserDiyInfos");

            migrationBuilder.RenameTable(
                name: "AppUserDiyInfos",
                newName: "UserDiyInfo");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "UserDiyInfo",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDiyInfo",
                table: "UserDiyInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_UserDiyInfo_DiyInfoId",
                table: "AppUsers",
                column: "DiyInfoId",
                principalTable: "UserDiyInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
