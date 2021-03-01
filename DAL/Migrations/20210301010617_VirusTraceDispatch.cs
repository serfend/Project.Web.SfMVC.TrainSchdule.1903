using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class VirusTraceDispatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TraceAlias",
                table: "Viruses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TraceTypeId",
                table: "Viruses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Viruses_TraceTypeId",
                table: "Viruses",
                column: "TraceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Viruses_VirusTraces_TraceTypeId",
                table: "Viruses",
                column: "TraceTypeId",
                principalTable: "VirusTraces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Viruses_VirusTraces_TraceTypeId",
                table: "Viruses");

            migrationBuilder.DropIndex(
                name: "IX_Viruses_TraceTypeId",
                table: "Viruses");

            migrationBuilder.DropColumn(
                name: "TraceAlias",
                table: "Viruses");

            migrationBuilder.DropColumn(
                name: "TraceTypeId",
                table: "Viruses");
        }
    }
}
