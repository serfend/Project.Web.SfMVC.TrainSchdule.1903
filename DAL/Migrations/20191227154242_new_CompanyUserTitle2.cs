using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class new_CompanyUserTitle2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RealName",
                table: "AppUserBaseInfos",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RealName",
                table: "AppUserBaseInfos",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
