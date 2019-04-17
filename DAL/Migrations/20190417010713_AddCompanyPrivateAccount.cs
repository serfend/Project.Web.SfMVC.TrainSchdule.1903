using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainSchdule.DAL.Migrations
{
    public partial class AddCompanyPrivateAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Companies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Companies");
        }
    }
}
