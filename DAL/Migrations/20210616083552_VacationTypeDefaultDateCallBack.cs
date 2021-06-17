using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class VacationTypeDefaultDateCallBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultDateRange",
                table: "VacationIndayTypes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultDateRange",
                table: "VacationIndayTypes");
        }
    }
}
