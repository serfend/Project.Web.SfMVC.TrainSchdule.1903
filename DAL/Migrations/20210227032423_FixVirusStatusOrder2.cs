using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class FixVirusStatusOrder2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 19,
                column: "Value",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 20,
                column: "Value",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 19,
                column: "Value",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 20,
                column: "Value",
                value: 1);
        }
    }
}
