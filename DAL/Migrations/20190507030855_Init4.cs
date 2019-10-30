using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "ApplyBaseInfos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DutiesName",
                table: "ApplyBaseInfos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "ApplyBaseInfos");

            migrationBuilder.DropColumn(
                name: "DutiesName",
                table: "ApplyBaseInfos");
        }
    }
}
