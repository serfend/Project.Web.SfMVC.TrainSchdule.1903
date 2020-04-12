using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class n413 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exist",
                table: "UserFileInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Exist",
                table: "UserFileInfos",
                nullable: false,
                defaultValue: false);
        }
    }
}
