using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class AccountStatus : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "PrivateAccount",
				table: "AppUserBaseInfos");

			migrationBuilder.AddColumn<string>(
				name: "CreateById",
				table: "UserFileInfos",
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "AccountStatus",
				table: "AppUsers",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<bool>(
				name: "PrivateAccount",
				table: "AppUsers",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<DateTime>(
				name: "StatusBeginDate",
				table: "AppUsers",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<DateTime>(
				name: "StatusEndDate",
				table: "AppUsers",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.UpdateData(
				table: "VacationTypes",
				keyColumn: "Id",
				keyValue: 4,
				column: "Description",
				value: @"仅限疫情期间14天隔离期使用，将不计算正休假。
其余情况请使用`确认时间`推迟归队，将从全年假期中扣除期间延迟归队的假期天数。");

			migrationBuilder.CreateIndex(
				name: "IX_UserFileInfos_CreateById",
				table: "UserFileInfos",
				column: "CreateById");

			migrationBuilder.AddForeignKey(
				name: "FK_UserFileInfos_AppUsers_CreateById",
				table: "UserFileInfos",
				column: "CreateById",
				principalTable: "AppUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_UserFileInfos_AppUsers_CreateById",
				table: "UserFileInfos");

			migrationBuilder.DropIndex(
				name: "IX_UserFileInfos_CreateById",
				table: "UserFileInfos");

			migrationBuilder.DropColumn(
				name: "CreateById",
				table: "UserFileInfos");

			migrationBuilder.DropColumn(
				name: "AccountStatus",
				table: "AppUsers");

			migrationBuilder.DropColumn(
				name: "PrivateAccount",
				table: "AppUsers");

			migrationBuilder.DropColumn(
				name: "StatusBeginDate",
				table: "AppUsers");

			migrationBuilder.DropColumn(
				name: "StatusEndDate",
				table: "AppUsers");

			migrationBuilder.AddColumn<bool>(
				name: "PrivateAccount",
				table: "AppUserBaseInfos",
				type: "bit",
				nullable: false,
				defaultValue: false);

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