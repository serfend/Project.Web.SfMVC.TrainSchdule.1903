using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class VacationType_NewProps : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "DIsabled",
				table: "VacationTypes",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<string>(
				name: "Description",
				table: "VacationTypes",
				nullable: true);

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 1,
				column: "Description",
				value: "正常休假");

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 2,
				column: "Description",
				value: "仅可在正休的假期结束后提交，不超过10天。");

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 3,
				column: "Description",
				value: "须提供医院开具的有效证明。");

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 4,
				column: "Description",
				value: @"仅限疫情期间14天隔离期使用，将不计算正休假。
其余情况请使用`确认时间`推迟归队，将从全年假期中扣除期间延迟归队的假期天数。");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "DIsabled",
				table: "VacationTypes");

			migrationBuilder.DropColumn(
				name: "Description",
				table: "VacationTypes");
		}
	}
}