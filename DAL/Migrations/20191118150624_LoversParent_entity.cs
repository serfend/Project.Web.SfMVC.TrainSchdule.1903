using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class LoversParent_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LoversParentId",
                table: "Settles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settles_LoversParentId",
                table: "Settles",
                column: "LoversParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Settles_Moment_LoversParentId",
                table: "Settles",
                column: "LoversParentId",
                principalTable: "Moment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Settles_Moment_LoversParentId",
                table: "Settles");

            migrationBuilder.DropIndex(
                name: "IX_Settles_LoversParentId",
                table: "Settles");

            migrationBuilder.DropColumn(
                name: "LoversParentId",
                table: "Settles");
        }
    }
}
