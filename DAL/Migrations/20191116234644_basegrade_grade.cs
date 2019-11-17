using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class basegrade_grade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseStandard",
                table: "Standards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseStandard",
                table: "Standards");
        }
    }
}
