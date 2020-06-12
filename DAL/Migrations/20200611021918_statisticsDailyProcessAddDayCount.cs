using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class statisticsDailyProcessAddDayCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteVacationExpectDayCount",
                table: "StatisticsAppliesProcesses");

            migrationBuilder.DropColumn(
                name: "CompleteVacationRealDayCount",
                table: "StatisticsAppliesProcesses");

            migrationBuilder.AddColumn<int>(
                name: "CompleteVacationExpectDayCount",
                table: "StatisticsDailyProcessRates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompleteVacationRealDayCount",
                table: "StatisticsDailyProcessRates",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteVacationExpectDayCount",
                table: "StatisticsDailyProcessRates");

            migrationBuilder.DropColumn(
                name: "CompleteVacationRealDayCount",
                table: "StatisticsDailyProcessRates");

            migrationBuilder.AddColumn<int>(
                name: "CompleteVacationExpectDayCount",
                table: "StatisticsAppliesProcesses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompleteVacationRealDayCount",
                table: "StatisticsAppliesProcesses",
                nullable: false,
                defaultValue: 0);
        }
    }
}
