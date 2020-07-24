using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class VacationType_newProp_background : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "Background",
				table: "VacationTypes",
				nullable: true);

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 1,
				column: "Background",
				value: "vacation_zhengxiu.jpg");

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 2,
				column: "Background",
				value: "vacation_shijia.jpg");

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 3,
				column: "Background",
				value: "vacation_bingxiu.jpg");

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 4,
				columns: new[] { "Background", "Description" },
				values: new object[] { "vacation_yiqingzhuanxiang.jpg", @"仅限疫情期间14天隔离期使用，将不计算正休假。
其余情况请使用`确认时间`推迟归队，将从全年假期中扣除期间延迟归队的假期天数。" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Background",
				table: "VacationTypes");

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 4,
				column: "Description",
				value: @"仅限疫情期间14天隔离期使用，将不计算正休假。
其余情况请使用`确认时间`推迟归队，将从全年假期中扣除期间延迟归队的假期天数。");
		}
	}
}