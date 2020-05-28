using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Statistics_DictByTitleType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VocationStatistics");

            migrationBuilder.DropTable(
                name: "VocationStatisticsDescriptions");

            migrationBuilder.DropTable(
                name: "VocationStatisticsDatas");

            migrationBuilder.DropTable(
                name: "VocationStatisticsDescriptionDataStatusCounts");

            migrationBuilder.AddColumn<string>(
                name: "TitleType",
                table: "UserCompanyTitles",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "AdminDivisions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longtitude",
                table: "AdminDivisions",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "VacationStatisticsDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    StatisticsId = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true)
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
                    Start = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    CurrentYear = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RootCompanyStatisticsId = table.Column<Guid>(nullable: true)
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
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    TitleType = table.Column<string>(nullable: true),
                    MembersCount = table.Column<int>(nullable: false),
                    CompleteYearlyVacationCount = table.Column<int>(nullable: false),
                    CompleteVacationExpectDayCount = table.Column<int>(nullable: false),
                    CompleteVacationRealDayCount = table.Column<int>(nullable: false),
                    MembersVacationDayLessThanP60 = table.Column<int>(nullable: false),
                    ApplyCount = table.Column<int>(nullable: false),
                    ApplyMembersCount = table.Column<int>(nullable: false),
                    ApplySumDayCount = table.Column<int>(nullable: false),
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VacationStatistics");

            migrationBuilder.DropTable(
                name: "VacationStatisticsDatas");

            migrationBuilder.DropTable(
                name: "VacationStatisticsDescriptions");

            migrationBuilder.DropColumn(
                name: "TitleType",
                table: "UserCompanyTitles");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AdminDivisions");

            migrationBuilder.DropColumn(
                name: "Longtitude",
                table: "AdminDivisions");

            migrationBuilder.CreateTable(
                name: "VocationStatisticsDescriptionDataStatusCounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Access = table.Column<int>(nullable: false),
                    Auditing = table.Column<int>(nullable: false),
                    Deny = table.Column<int>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocationStatisticsDescriptionDataStatusCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VocationStatisticsDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplyCountId = table.Column<Guid>(nullable: true),
                    ApplyMembersCountId = table.Column<Guid>(nullable: true),
                    ApplySumDayCountId = table.Column<Guid>(nullable: true),
                    CompleteVacationExpectDayCount = table.Column<int>(nullable: false),
                    CompleteVacationRealDayCount = table.Column<int>(nullable: false),
                    CompleteYearlyVacationCount = table.Column<int>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    MembersCount = table.Column<int>(nullable: false),
                    MembersVacationDayLessThanP60 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocationStatisticsDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplyCountId",
                        column: x => x.ApplyCountId,
                        principalTable: "VocationStatisticsDescriptionDataStatusCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplyMembersCountId",
                        column: x => x.ApplyMembersCountId,
                        principalTable: "VocationStatisticsDescriptionDataStatusCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplySumDayCountId",
                        column: x => x.ApplySumDayCountId,
                        principalTable: "VocationStatisticsDescriptionDataStatusCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VocationStatisticsDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyCode = table.Column<string>(nullable: true),
                    CurrentLevelStatisticsId = table.Column<Guid>(nullable: true),
                    IncludeChildLevelStatisticsId = table.Column<Guid>(nullable: true),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    StatisticsId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocationStatisticsDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDescriptions_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDescriptions_VocationStatisticsDatas_CurrentLevelStatisticsId",
                        column: x => x.CurrentLevelStatisticsId,
                        principalTable: "VocationStatisticsDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VocationStatisticsDescriptions_VocationStatisticsDatas_IncludeChildLevelStatisticsId",
                        column: x => x.IncludeChildLevelStatisticsId,
                        principalTable: "VocationStatisticsDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VocationStatistics",
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
                    table.PrimaryKey("PK_VocationStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocationStatistics_VocationStatisticsDescriptions_RootCompanyStatisticsId",
                        column: x => x.RootCompanyStatisticsId,
                        principalTable: "VocationStatisticsDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatistics_RootCompanyStatisticsId",
                table: "VocationStatistics",
                column: "RootCompanyStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDatas_ApplyCountId",
                table: "VocationStatisticsDatas",
                column: "ApplyCountId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDatas_ApplyMembersCountId",
                table: "VocationStatisticsDatas",
                column: "ApplyMembersCountId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDatas_ApplySumDayCountId",
                table: "VocationStatisticsDatas",
                column: "ApplySumDayCountId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDescriptions_CompanyCode",
                table: "VocationStatisticsDescriptions",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDescriptions_CurrentLevelStatisticsId",
                table: "VocationStatisticsDescriptions",
                column: "CurrentLevelStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_VocationStatisticsDescriptions_IncludeChildLevelStatisticsId",
                table: "VocationStatisticsDescriptions",
                column: "IncludeChildLevelStatisticsId");
        }
    }
}
