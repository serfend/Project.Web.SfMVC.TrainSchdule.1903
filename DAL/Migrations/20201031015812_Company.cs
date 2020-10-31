using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordModefy",
                table: "AppUserBaseInfos",
                newName: "PasswodModify");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswodModify",
                table: "AppUserBaseInfos",
                newName: "PasswordModefy");
        }
    }
}
