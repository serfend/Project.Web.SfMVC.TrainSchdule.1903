using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ShareBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCodes_GameR3Users_ShareById",
                table: "GiftCodes");

            migrationBuilder.DropIndex(
                name: "IX_GiftCodes_ShareById",
                table: "GiftCodes");

            migrationBuilder.DropColumn(
                name: "ShareById",
                table: "GiftCodes");

            migrationBuilder.AddColumn<string>(
                name: "ShareBy",
                table: "GiftCodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareBy",
                table: "GiftCodes");

            migrationBuilder.AddColumn<Guid>(
                name: "ShareById",
                table: "GiftCodes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GiftCodes_ShareById",
                table: "GiftCodes",
                column: "ShareById");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCodes_GameR3Users_ShareById",
                table: "GiftCodes",
                column: "ShareById",
                principalTable: "GameR3Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
