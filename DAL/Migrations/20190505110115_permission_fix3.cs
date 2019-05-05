using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class permission_fix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthKey",
                table: "UserBaseInfo");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Permissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Permissions");

            migrationBuilder.AddColumn<string>(
                name: "AuthKey",
                table: "UserBaseInfo",
                nullable: true);
        }
    }
}
