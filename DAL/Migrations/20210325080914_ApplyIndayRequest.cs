using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ApplyIndayRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppliesInday_ApplyIndayRequest_RequestInfoId",
                table: "AppliesInday");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyIndayRequest_AdminDivisions_VacationPlaceCode",
                table: "ApplyIndayRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyIndayRequest",
                table: "ApplyIndayRequest");

            migrationBuilder.RenameTable(
                name: "ApplyIndayRequest",
                newName: "ApplyIndayRequests");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyIndayRequest_VacationPlaceCode",
                table: "ApplyIndayRequests",
                newName: "IX_ApplyIndayRequests_VacationPlaceCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "ApplyIndayRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyIndayRequests",
                table: "ApplyIndayRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppliesInday_ApplyIndayRequests_RequestInfoId",
                table: "AppliesInday",
                column: "RequestInfoId",
                principalTable: "ApplyIndayRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyIndayRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyIndayRequests",
                column: "VacationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppliesInday_ApplyIndayRequests_RequestInfoId",
                table: "AppliesInday");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyIndayRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyIndayRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyIndayRequests",
                table: "ApplyIndayRequests");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "ApplyIndayRequests");

            migrationBuilder.RenameTable(
                name: "ApplyIndayRequests",
                newName: "ApplyIndayRequest");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyIndayRequests_VacationPlaceCode",
                table: "ApplyIndayRequest",
                newName: "IX_ApplyIndayRequest_VacationPlaceCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyIndayRequest",
                table: "ApplyIndayRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppliesInday_ApplyIndayRequest_RequestInfoId",
                table: "AppliesInday",
                column: "RequestInfoId",
                principalTable: "ApplyIndayRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyIndayRequest_AdminDivisions_VacationPlaceCode",
                table: "ApplyIndayRequest",
                column: "VacationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
