using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class vocationStatistics2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatistics_VocationStatisticsDescription_RootCompanyStatisticsId",
                table: "VocationStatistics");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatisticsDescription_Companies_CompanyCode",
                table: "VocationStatisticsDescription");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatisticsDescription_VocationStatisticsData_CurrentLevelStatisticsId",
                table: "VocationStatisticsDescription");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatisticsDescription_VocationStatisticsData_IncludeChildLevelStatisticsId",
                table: "VocationStatisticsDescription");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatisticsDescription_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                table: "VocationStatisticsDescription");

            migrationBuilder.DropIndex(
                name: "IX_Applies_VocationStatisticsDescriptionId",
                table: "Applies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VocationStatisticsDescription",
                table: "VocationStatisticsDescription");

            migrationBuilder.DropColumn(
                name: "VocationStatisticsDescriptionId",
                table: "Applies");

            migrationBuilder.RenameTable(
                name: "VocationStatisticsDescription",
                newName: "VocationStatisticsDescriptions");

            migrationBuilder.RenameIndex(
                name: "IX_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                table: "VocationStatisticsDescriptions",
                newName: "IX_VocationStatisticsDescriptions_VocationStatisticsDescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_VocationStatisticsDescription_IncludeChildLevelStatisticsId",
                table: "VocationStatisticsDescriptions",
                newName: "IX_VocationStatisticsDescriptions_IncludeChildLevelStatisticsId");

            migrationBuilder.RenameIndex(
                name: "IX_VocationStatisticsDescription_CurrentLevelStatisticsId",
                table: "VocationStatisticsDescriptions",
                newName: "IX_VocationStatisticsDescriptions_CurrentLevelStatisticsId");

            migrationBuilder.RenameIndex(
                name: "IX_VocationStatisticsDescription_CompanyCode",
                table: "VocationStatisticsDescriptions",
                newName: "IX_VocationStatisticsDescriptions_CompanyCode");

            migrationBuilder.AddColumn<string>(
                name: "StatisticsId",
                table: "VocationStatisticsDescriptions",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VocationStatisticsDescriptions",
                table: "VocationStatisticsDescriptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatistics_VocationStatisticsDescriptions_RootCompanyStatisticsId",
                table: "VocationStatistics",
                column: "RootCompanyStatisticsId",
                principalTable: "VocationStatisticsDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatisticsDescriptions_Companies_CompanyCode",
                table: "VocationStatisticsDescriptions",
                column: "CompanyCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatisticsDescriptions_VocationStatisticsData_CurrentLevelStatisticsId",
                table: "VocationStatisticsDescriptions",
                column: "CurrentLevelStatisticsId",
                principalTable: "VocationStatisticsData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatisticsDescriptions_VocationStatisticsData_IncludeChildLevelStatisticsId",
                table: "VocationStatisticsDescriptions",
                column: "IncludeChildLevelStatisticsId",
                principalTable: "VocationStatisticsData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatisticsDescriptions_VocationStatisticsDescriptions_VocationStatisticsDescriptionId",
                table: "VocationStatisticsDescriptions",
                column: "VocationStatisticsDescriptionId",
                principalTable: "VocationStatisticsDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatistics_VocationStatisticsDescriptions_RootCompanyStatisticsId",
                table: "VocationStatistics");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatisticsDescriptions_Companies_CompanyCode",
                table: "VocationStatisticsDescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatisticsDescriptions_VocationStatisticsData_CurrentLevelStatisticsId",
                table: "VocationStatisticsDescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatisticsDescriptions_VocationStatisticsData_IncludeChildLevelStatisticsId",
                table: "VocationStatisticsDescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_VocationStatisticsDescriptions_VocationStatisticsDescriptions_VocationStatisticsDescriptionId",
                table: "VocationStatisticsDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VocationStatisticsDescriptions",
                table: "VocationStatisticsDescriptions");

            migrationBuilder.DropColumn(
                name: "StatisticsId",
                table: "VocationStatisticsDescriptions");

            migrationBuilder.RenameTable(
                name: "VocationStatisticsDescriptions",
                newName: "VocationStatisticsDescription");

            migrationBuilder.RenameIndex(
                name: "IX_VocationStatisticsDescriptions_VocationStatisticsDescriptionId",
                table: "VocationStatisticsDescription",
                newName: "IX_VocationStatisticsDescription_VocationStatisticsDescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_VocationStatisticsDescriptions_IncludeChildLevelStatisticsId",
                table: "VocationStatisticsDescription",
                newName: "IX_VocationStatisticsDescription_IncludeChildLevelStatisticsId");

            migrationBuilder.RenameIndex(
                name: "IX_VocationStatisticsDescriptions_CurrentLevelStatisticsId",
                table: "VocationStatisticsDescription",
                newName: "IX_VocationStatisticsDescription_CurrentLevelStatisticsId");

            migrationBuilder.RenameIndex(
                name: "IX_VocationStatisticsDescriptions_CompanyCode",
                table: "VocationStatisticsDescription",
                newName: "IX_VocationStatisticsDescription_CompanyCode");

            migrationBuilder.AddColumn<Guid>(
                name: "VocationStatisticsDescriptionId",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VocationStatisticsDescription",
                table: "VocationStatisticsDescription",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_VocationStatisticsDescriptionId",
                table: "Applies",
                column: "VocationStatisticsDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                table: "Applies",
                column: "VocationStatisticsDescriptionId",
                principalTable: "VocationStatisticsDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatistics_VocationStatisticsDescription_RootCompanyStatisticsId",
                table: "VocationStatistics",
                column: "RootCompanyStatisticsId",
                principalTable: "VocationStatisticsDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatisticsDescription_Companies_CompanyCode",
                table: "VocationStatisticsDescription",
                column: "CompanyCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatisticsDescription_VocationStatisticsData_CurrentLevelStatisticsId",
                table: "VocationStatisticsDescription",
                column: "CurrentLevelStatisticsId",
                principalTable: "VocationStatisticsData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatisticsDescription_VocationStatisticsData_IncludeChildLevelStatisticsId",
                table: "VocationStatisticsDescription",
                column: "IncludeChildLevelStatisticsId",
                principalTable: "VocationStatisticsData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VocationStatisticsDescription_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                table: "VocationStatisticsDescription",
                column: "VocationStatisticsDescriptionId",
                principalTable: "VocationStatisticsDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
