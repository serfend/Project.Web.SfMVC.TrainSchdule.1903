using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class checkIfNotCacuStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotCaculateStatistics",
                table: "DutyTypes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotCaculateStatistics",
                table: "DutyTypes");
        }
    }
}
