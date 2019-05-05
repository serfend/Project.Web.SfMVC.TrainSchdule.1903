using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class duties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCompanyInfo_Duties_DutiesId",
                table: "UserCompanyInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserCompanyInfo_DutiesId",
                table: "UserCompanyInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Duties",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "DutiesId",
                table: "UserCompanyInfo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Duties");

            migrationBuilder.AddColumn<string>(
                name: "DutiesCode",
                table: "UserCompanyInfo",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Duties",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Duties",
                table: "Duties",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyInfo_DutiesCode",
                table: "UserCompanyInfo",
                column: "DutiesCode");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCompanyInfo_Duties_DutiesCode",
                table: "UserCompanyInfo",
                column: "DutiesCode",
                principalTable: "Duties",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCompanyInfo_Duties_DutiesCode",
                table: "UserCompanyInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserCompanyInfo_DutiesCode",
                table: "UserCompanyInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Duties",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "DutiesCode",
                table: "UserCompanyInfo");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Duties");

            migrationBuilder.AddColumn<Guid>(
                name: "DutiesId",
                table: "UserCompanyInfo",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Duties",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Duties",
                table: "Duties",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyInfo_DutiesId",
                table: "UserCompanyInfo",
                column: "DutiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCompanyInfo_Duties_DutiesId",
                table: "UserCompanyInfo",
                column: "DutiesId",
                principalTable: "Duties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
