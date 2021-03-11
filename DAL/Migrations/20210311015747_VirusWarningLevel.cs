using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class VirusWarningLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "WarningLevel",
                table: "VirusTraces",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarningLevel",
                table: "VirusTraces");
        }
    }
}
