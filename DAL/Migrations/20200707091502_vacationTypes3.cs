using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class vacationTypes3 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "CaculateBenefit",
				table: "VacationTypes",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "CanUseOnTrip",
				table: "VacationTypes",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "MinusNextYear",
				table: "VacationTypes",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<bool>(
				name: "NotPermitCrossYear",
				table: "VacationTypes",
				nullable: false,
				defaultValue: false);

			migrationBuilder.InsertData(
				table: "VacationTypes",
				columns: new[] { "Id", "Alias", "CaculateBenefit", "CanUseOnTrip", "IsRemoved", "IsRemovedDate", "MaxLength", "MinLength", "MinusNextYear", "Name", "NotPermitCrossYear", "Primary", "RegionOnCompany" },
				values: new object[,]
				{
					{ 1, "正休", true, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 500, 0, false, "正休", false, true, "" },
					{ 2, "事假", false, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 0, true, "事假", false, false, "" },
					{ 3, "病休", false, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 0, true, "病休", false, false, "" },
					{ 4, "疫情专项", false, false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 0, true, "疫情专项", false, false, "" }
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 4);

			migrationBuilder.DropColumn(
				name: "CaculateBenefit",
				table: "VacationTypes");

			migrationBuilder.DropColumn(
				name: "CanUseOnTrip",
				table: "VacationTypes");

			migrationBuilder.DropColumn(
				name: "MinusNextYear",
				table: "VacationTypes");

			migrationBuilder.DropColumn(
				name: "NotPermitCrossYear",
				table: "VacationTypes");
		}
	}
}