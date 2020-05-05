using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class indexofUserAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserActions_Date",
                table: "UserActions",
                column: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserActions_Date",
                table: "UserActions");
        }
    }
}
