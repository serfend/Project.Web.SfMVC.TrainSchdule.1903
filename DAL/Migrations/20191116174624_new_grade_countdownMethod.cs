using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class new_grade_countdownMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullGrade",
                table: "Standard");

            migrationBuilder.AddColumn<bool>(
                name: "CountDown",
                table: "Subjects",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountDown",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "FullGrade",
                table: "Standard",
                nullable: false,
                defaultValue: 0);
        }
    }
}
