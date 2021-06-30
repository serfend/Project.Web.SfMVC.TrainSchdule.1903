using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class StatisticsAppliesRankCompanyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserCompany",
                table: "StatisticsApplyRanks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCompany",
                table: "StatisticsApplyRanks");
        }
    }
}
