using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class diyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AppUserBaseInfos");

            migrationBuilder.DropColumn(
                name: "About",
                table: "AppUserApplicationInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "DiyInfoId",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cid",
                table: "AppUserBaseInfos",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserDiyInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    About = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiyInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_DiyInfoId",
                table: "AppUsers",
                column: "DiyInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_UserDiyInfo_DiyInfoId",
                table: "AppUsers",
                column: "DiyInfoId",
                principalTable: "UserDiyInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_UserDiyInfo_DiyInfoId",
                table: "AppUsers");

            migrationBuilder.DropTable(
                name: "UserDiyInfo");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_DiyInfoId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DiyInfoId",
                table: "AppUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Cid",
                table: "AppUserBaseInfos",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AppUserBaseInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "AppUserApplicationInfos",
                nullable: true);
        }
    }
}
