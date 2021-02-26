using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ClientVirusHandleRomveClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientMachineId",
                table: "VirusHandleRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientMachineId",
                table: "VirusHandleRecords",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
