using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ClientVirusStausColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 32,
                column: "Color",
                value: "#20b2aaff");

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 33,
                column: "Color",
                value: "#90ee90ff");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 32,
                column: "Color",
                value: "#f0f8ffff");

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 33,
                column: "Color",
                value: "#f0f8ffff");
        }
    }
}
