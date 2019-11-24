using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class vocationStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VocationStatisticsDescriptionId",
                table: "Applies",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VocationStatisticsDescriptionDataStatusCount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Access = table.Column<int>(nullable: false),
                    Deny = table.Column<int>(nullable: false),
                    Auditing = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocationStatisticsDescriptionDataStatusCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VocationStatisticsData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MembersCount = table.Column<int>(nullable: false),
                    CompleteYearlyVocationCount = table.Column<int>(nullable: false),
                    CompleteVocationExpectDayCount = table.Column<int>(nullable: false),
                    CompleteVocationRealDayCount = table.Column<int>(nullable: false),
                    MembersVocationDayLessThanP60 = table.Column<int>(nullable: false),
                    ApplyCountId = table.Column<Guid>(nullable: true),
                    ApplyMembersCountId = table.Column<Guid>(nullable: true),
                    ApplySumDayCountId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocationStatisticsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplyCountId",
                        column: x => x.ApplyCountId,
                        principalTable: "VocationStatisticsDescriptionDataStatusCount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplyMembersCountId",
                        column: x => x.ApplyMembersCountId,
                        principalTable: "VocationStatisticsDescriptionDataStatusCount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplySumDayCountId",
                        column: x => x.ApplySumDayCountId,
                        principalTable: "VocationStatisticsDescriptionDataStatusCount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VocationStatisticsDescription",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyCode = table.Column<string>(nullable: true),
                    CurrentLevelStatisticsId = table.Column<Guid>(nullable: true),
                    IncludeChildLevelStatisticsId = table.Column<Guid>(nullable: true),
                    VocationStatisticsDescriptionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocationStatisticsDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDescription_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDescription_VocationStatisticsData_CurrentLevelStatisticsId",
                        column: x => x.CurrentLevelStatisticsId,
                        principalTable: "VocationStatisticsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDescription_VocationStatisticsData_IncludeChildLevelStatisticsId",
                        column: x => x.IncludeChildLevelStatisticsId,
                        principalTable: "VocationStatisticsData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDescription_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                        column: x => x.VocationStatisticsDescriptionId,
                        principalTable: "VocationStatisticsDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VocationStatistics",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    CurrentYear = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RootCompanyStatisticsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocationStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocationStatistics_VocationStatisticsDescription_RootCompanyStatisticsId",
                        column: x => x.RootCompanyStatisticsId,
                        principalTable: "VocationStatisticsDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applies_VocationStatisticsDescriptionId",
                table: "Applies",
                column: "VocationStatisticsDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatistics_RootCompanyStatisticsId",
                table: "VocationStatistics",
                column: "RootCompanyStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsData_ApplyCountId",
                table: "VocationStatisticsData",
                column: "ApplyCountId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsData_ApplyMembersCountId",
                table: "VocationStatisticsData",
                column: "ApplyMembersCountId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsData_ApplySumDayCountId",
                table: "VocationStatisticsData",
                column: "ApplySumDayCountId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDescription_CompanyCode",
                table: "VocationStatisticsDescription",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDescription_CurrentLevelStatisticsId",
                table: "VocationStatisticsDescription",
                column: "CurrentLevelStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDescription_IncludeChildLevelStatisticsId",
                table: "VocationStatisticsDescription",
                column: "IncludeChildLevelStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                table: "VocationStatisticsDescription",
                column: "VocationStatisticsDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                table: "Applies",
                column: "VocationStatisticsDescriptionId",
                principalTable: "VocationStatisticsDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_VocationStatisticsDescription_VocationStatisticsDescriptionId",
                table: "Applies");

            migrationBuilder.DropTable(
                name: "VocationStatistics");

            migrationBuilder.DropTable(
                name: "VocationStatisticsDescription");

            migrationBuilder.DropTable(
                name: "VocationStatisticsData");

            migrationBuilder.DropTable(
                name: "VocationStatisticsDescriptionDataStatusCount");

            migrationBuilder.DropIndex(
                name: "IX_Applies_VocationStatisticsDescriptionId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "VocationStatisticsDescriptionId",
                table: "Applies");
        }
    }
}
