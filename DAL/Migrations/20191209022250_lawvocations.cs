using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class lawvocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplyRequestId1",
                table: "VocationAdditionals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VocationAdditionals_ApplyRequestId1",
                table: "VocationAdditionals",
                column: "ApplyRequestId1");

            migrationBuilder.AddForeignKey(
                name: "FK_VocationAdditionals_ApplyRequests_ApplyRequestId1",
                table: "VocationAdditionals",
                column: "ApplyRequestId1",
                principalTable: "ApplyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VocationAdditionals_ApplyRequests_ApplyRequestId1",
                table: "VocationAdditionals");

            migrationBuilder.DropIndex(
                name: "IX_VocationAdditionals_ApplyRequestId1",
                table: "VocationAdditionals");

            migrationBuilder.DropColumn(
                name: "ApplyRequestId1",
                table: "VocationAdditionals");
        }
    }
}
