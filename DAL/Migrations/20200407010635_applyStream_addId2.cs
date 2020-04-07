using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class applyStream_addId2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "UserActions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_Ip",
                table: "UserActions",
                column: "Ip");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserActions_Ip",
                table: "UserActions");

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "UserActions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
