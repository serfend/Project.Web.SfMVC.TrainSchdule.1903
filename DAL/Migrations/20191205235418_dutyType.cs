using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class dutyType : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "PrevYearlyComsumeLength",
				table: "Settles",
				nullable: false,
				defaultValue: 0);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "PrevYearlyComsumeLength",
				table: "Settles");
		}
	}
}