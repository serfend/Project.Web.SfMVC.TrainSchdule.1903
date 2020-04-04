using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class nowAuditCompanyName : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "NowAuditCompanyName",
				table: "Applies",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "NowAuditCompanyName",
				table: "Applies");
		}
	}
}