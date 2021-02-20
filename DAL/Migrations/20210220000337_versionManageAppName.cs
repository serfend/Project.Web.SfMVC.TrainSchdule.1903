using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class versionManageAppName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppName",
                table: "ApplicationUpdateRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppName",
                table: "ApplicationUpdateRecords");
        }
    }
}
