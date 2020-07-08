using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class vacationTypes_AllowBeforePrimary : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "AllowBeforePrimary",
				table: "VacationTypes",
				nullable: false,
				defaultValue: false);

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 3,
				column: "AllowBeforePrimary",
				value: true);

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 4,
				column: "AllowBeforePrimary",
				value: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "AllowBeforePrimary",
				table: "VacationTypes");
		}
	}
}