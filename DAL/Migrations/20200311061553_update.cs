using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComboTimes",
                table: "SignIns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "LastSignInId",
                table: "GameR3Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameR3Users_LastSignInId",
                table: "GameR3Users",
                column: "LastSignInId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameR3Users_SignIns_LastSignInId",
                table: "GameR3Users",
                column: "LastSignInId",
                principalTable: "SignIns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameR3Users_SignIns_LastSignInId",
                table: "GameR3Users");

            migrationBuilder.DropIndex(
                name: "IX_GameR3Users_LastSignInId",
                table: "GameR3Users");

            migrationBuilder.DropColumn(
                name: "ComboTimes",
                table: "SignIns");

            migrationBuilder.DropColumn(
                name: "LastSignInId",
                table: "GameR3Users");
        }
    }
}
