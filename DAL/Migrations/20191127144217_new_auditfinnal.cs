using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class new_auditfinnal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecallOrders_Applies_ApplyId",
                table: "RecallOrders");

            migrationBuilder.DropIndex(
                name: "IX_RecallOrders_ApplyId",
                table: "RecallOrders");

            migrationBuilder.DropColumn(
                name: "ApplyId",
                table: "RecallOrders");

            migrationBuilder.AddColumn<string>(
                name: "FinnalAuditCompany",
                table: "ApplyBaseInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinnalAuditCompany",
                table: "Applies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinnalAuditCompany",
                table: "ApplyBaseInfos");

            migrationBuilder.DropColumn(
                name: "FinnalAuditCompany",
                table: "Applies");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyId",
                table: "RecallOrders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecallOrders_ApplyId",
                table: "RecallOrders",
                column: "ApplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecallOrders_Applies_ApplyId",
                table: "RecallOrders",
                column: "ApplyId",
                principalTable: "Applies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
