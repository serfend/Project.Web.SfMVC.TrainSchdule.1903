using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class StatisticsProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VacationStatistics");

            migrationBuilder.DropTable(
                name: "VacationStatisticsDatas");

            migrationBuilder.DropTable(
                name: "VacationStatisticsDescriptions");

            migrationBuilder.CreateTable(
                name: "StatisticsAppliesProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MembersCount = table.Column<int>(nullable: false),
                    CompleteYearlyVacationCount = table.Column<int>(nullable: false),
                    CompleteVacationExpectDayCount = table.Column<int>(nullable: false),
                    CompleteVacationRealDayCount = table.Column<int>(nullable: false),
                    MembersVacationDayLessThanP60 = table.Column<int>(nullable: false),
                    ApplyCount = table.Column<int>(nullable: false),
                    ApplyMembersCount = table.Column<int>(nullable: false),
                    ApplySumDayCount = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    Target = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsAppliesProcesses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticsAppliesProcesses");

            migrationBuilder.CreateTable(
                name: "VacationStatisticsDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyCode = table.Column<string>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    StatisticsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationStatisticsDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationStatisticsDescriptions_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VacationStatistics",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CurrentYear = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    End = table.Column<DateTime>(nullable: false),
                    RootCompanyStatisticsId = table.Column<Guid>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationStatistics_VacationStatisticsDescriptions_RootCompanyStatisticsId",
                        column: x => x.RootCompanyStatisticsId,
                        principalTable: "VacationStatisticsDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VacationStatisticsDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplyCount = table.Column<int>(nullable: false),
                    ApplyMembersCount = table.Column<int>(nullable: false),
                    ApplySumDayCount = table.Column<int>(nullable: false),
                    CompleteVacationExpectDayCount = table.Column<int>(nullable: false),
                    CompleteVacationRealDayCount = table.Column<int>(nullable: false),
                    CompleteYearlyVacationCount = table.Column<int>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    MembersCount = table.Column<int>(nullable: false),
                    MembersVacationDayLessThanP60 = table.Column<int>(nullable: false),
                    TitleType = table.Column<string>(nullable: true),
                    VacationStatisticsDescriptionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationStatisticsDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationStatisticsDatas_VacationStatisticsDescriptions_VacationStatisticsDescriptionId",
                        column: x => x.VacationStatisticsDescriptionId,
                        principalTable: "VacationStatisticsDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VacationStatistics_RootCompanyStatisticsId",
                table: "VacationStatistics",
                column: "RootCompanyStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationStatisticsDatas_VacationStatisticsDescriptionId",
                table: "VacationStatisticsDatas",
                column: "VacationStatisticsDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationStatisticsDescriptions_CompanyCode",
                table: "VacationStatisticsDescriptions",
                column: "CompanyCode");
        }
    }
}
