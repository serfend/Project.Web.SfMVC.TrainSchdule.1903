using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainSchdule.DAL.Migrations
{
    public partial class addCompanySupportFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Companys",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Companys",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Companys",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companys_ParentId",
                table: "Companys",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companys_Companys_ParentId",
                table: "Companys",
                column: "ParentId",
                principalTable: "Companys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companys_Companys_ParentId",
                table: "Companys");

            migrationBuilder.DropIndex(
                name: "IX_Companys_ParentId",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Companys");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Companys");
        }
    }
}
