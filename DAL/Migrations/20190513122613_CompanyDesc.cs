using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class CompanyDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyParentTypeDesc",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTypeDesc",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyParentTypeDesc",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanyTypeDesc",
                table: "Companies");
        }
    }
}
