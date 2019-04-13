using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainSchdule.DAL.Migrations
{
    public partial class addCompaniesupportFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Companies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ParentId",
                table: "Companies",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Companies_ParentId",
                table: "Companies",
                column: "ParentId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Companies_ParentId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ParentId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Companies");
        }
    }
}
