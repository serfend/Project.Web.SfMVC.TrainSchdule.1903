using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class DutiesLevel : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "Level",
				table: "Duties",
				nullable: false,
				defaultValue: 0);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Level",
				table: "Duties");
		}
	}
}