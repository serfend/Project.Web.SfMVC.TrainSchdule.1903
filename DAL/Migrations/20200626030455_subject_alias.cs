using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class subject_alias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Subjects",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "StatisticsNewApplies",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "StatisticsDailyProcessRates",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "StatisticsCompleteApplies",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "StatisticsAppliesProcesses",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsNewApplies_Target_CompanyCode",
                table: "StatisticsNewApplies",
                columns: new[] { "Target", "CompanyCode" });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsDailyProcessRates_Target_CompanyCode",
                table: "StatisticsDailyProcessRates",
                columns: new[] { "Target", "CompanyCode" });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsCompleteApplies_Target_CompanyCode",
                table: "StatisticsCompleteApplies",
                columns: new[] { "Target", "CompanyCode" });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsAppliesProcesses_Target_CompanyCode",
                table: "StatisticsAppliesProcesses",
                columns: new[] { "Target", "CompanyCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StatisticsNewApplies_Target_CompanyCode",
                table: "StatisticsNewApplies");

            migrationBuilder.DropIndex(
                name: "IX_StatisticsDailyProcessRates_Target_CompanyCode",
                table: "StatisticsDailyProcessRates");

            migrationBuilder.DropIndex(
                name: "IX_StatisticsCompleteApplies_Target_CompanyCode",
                table: "StatisticsCompleteApplies");

            migrationBuilder.DropIndex(
                name: "IX_StatisticsAppliesProcesses_Target_CompanyCode",
                table: "StatisticsAppliesProcesses");

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Subjects");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "StatisticsNewApplies",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "StatisticsDailyProcessRates",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "StatisticsCompleteApplies",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyCode",
                table: "StatisticsAppliesProcesses",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
