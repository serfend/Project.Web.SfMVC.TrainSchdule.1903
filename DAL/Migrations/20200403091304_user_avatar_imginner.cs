using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class user_avatar_imginner : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<byte[]>(
				name: "Img",
				table: "AppUserDiyAvatars",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "UserId",
				table: "AppUserDiyAvatars",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_AppUserDiyAvatars_UserId",
				table: "AppUserDiyAvatars",
				column: "UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserDiyAvatars_AppUsers_UserId",
				table: "AppUserDiyAvatars",
				column: "UserId",
				principalTable: "AppUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUserDiyAvatars_AppUsers_UserId",
				table: "AppUserDiyAvatars");

			migrationBuilder.DropIndex(
				name: "IX_AppUserDiyAvatars_UserId",
				table: "AppUserDiyAvatars");

			migrationBuilder.DropColumn(
				name: "Img",
				table: "AppUserDiyAvatars");

			migrationBuilder.DropColumn(
				name: "UserId",
				table: "AppUserDiyAvatars");
		}
	}
}