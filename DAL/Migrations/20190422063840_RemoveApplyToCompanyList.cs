using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class RemoveApplyToCompanyList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropForeignKey(
				name: "FK_Companies_Applies_ApplyId",
				table: "Companies");

			migrationBuilder.DropIndex(
				name: "IX_Companies_ApplyId",
				table: "Companies");

			migrationBuilder.DropColumn(
				name: "ApplyId",
				table: "Companies");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplyId",
                table: "Companies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ApplyId",
                table: "Companies",
                column: "ApplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Applies_ApplyId",
                table: "Companies",
                column: "ApplyId",
                principalTable: "Applies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
