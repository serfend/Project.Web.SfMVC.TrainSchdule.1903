using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class user_diy_thirdpardAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThirdpardAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    Account = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    ThirdpardPlatformName = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    UserDiyInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdpardAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThirdpardAccount_AppUserDiyInfos_UserDiyInfoId",
                        column: x => x.UserDiyInfoId,
                        principalTable: "AppUserDiyInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThirdpardAccount_UserDiyInfoId",
                table: "ThirdpardAccount",
                column: "UserDiyInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThirdpardAccount");
        }
    }
}
