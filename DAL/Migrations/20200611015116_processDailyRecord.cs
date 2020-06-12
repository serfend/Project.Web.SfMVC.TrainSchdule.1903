using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class processDailyRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplyMembersCount",
                table: "StatisticsAppliesProcesses");

            migrationBuilder.DropColumn(
                name: "CompleteYearlyVacationCount",
                table: "StatisticsAppliesProcesses");

            migrationBuilder.DropColumn(
                name: "MembersCount",
                table: "StatisticsAppliesProcesses");

            migrationBuilder.DropColumn(
                name: "MembersVacationDayLessThanP60",
                table: "StatisticsAppliesProcesses");

            migrationBuilder.CreateTable(
                name: "StatisticsDailyProcessRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MembersCount = table.Column<int>(nullable: false),
                    CompleteYearlyVacationCount = table.Column<int>(nullable: false),
                    MembersVacationDayLessThanP60 = table.Column<int>(nullable: false),
                    ApplyMembersCount = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    Target = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsDailyProcessRates", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticsDailyProcessRates");

            migrationBuilder.AddColumn<int>(
                name: "ApplyMembersCount",
                table: "StatisticsAppliesProcesses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompleteYearlyVacationCount",
                table: "StatisticsAppliesProcesses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MembersCount",
                table: "StatisticsAppliesProcesses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MembersVacationDayLessThanP60",
                table: "StatisticsAppliesProcesses",
                nullable: false,
                defaultValue: 0);
        }
    }
}
