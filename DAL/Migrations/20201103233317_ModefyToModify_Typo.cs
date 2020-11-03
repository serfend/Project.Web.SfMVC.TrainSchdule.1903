using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class ModefyToModify_Typo : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUsersSettleModefyRecord_AppUserSocialInfoSettles_SettleId",
				table: "AppUsersSettleModefyRecord");

			migrationBuilder.DropPrimaryKey(
				name: "PK_AppUsersSettleModefyRecord",
				table: "AppUsersSettleModefyRecord");

			migrationBuilder.RenameTable(
				name: "AppUsersSettleModefyRecord",
				newName: "AppUsersSettleModifyRecord");

			migrationBuilder.RenameIndex(
				name: "IX_AppUsersSettleModefyRecord_SettleId",
				table: "AppUsersSettleModifyRecord",
				newName: "IX_AppUsersSettleModifyRecord_SettleId");

			migrationBuilder.RenameColumn(
				name: "LastModefy",
				table: "UserFileInfos",
				newName: "LastModify");

			migrationBuilder.AddPrimaryKey(
				name: "PK_AppUsersSettleModifyRecord",
				table: "AppUsersSettleModifyRecord",
				column: "Code");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUsersSettleModifyRecord_AppUserSocialInfoSettles_SettleId",
				table: "AppUsersSettleModifyRecord",
				column: "SettleId",
				principalTable: "AppUserSocialInfoSettles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUsersSettleModifyRecord_AppUserSocialInfoSettles_SettleId",
				table: "AppUsersSettleModifyRecord");

			migrationBuilder.DropPrimaryKey(
				name: "PK_AppUsersSettleModifyRecord",
				table: "AppUsersSettleModifyRecord");

			migrationBuilder.RenameTable(
				name: "AppUsersSettleModifyRecord",
				newName: "AppUsersSettleModefyRecord");

			migrationBuilder.RenameIndex(
				name: "IX_AppUsersSettleModifyRecord_SettleId",
				table: "AppUsersSettleModefyRecord",
				newName: "IX_AppUsersSettleModefyRecord_SettleId");

			migrationBuilder.RenameColumn(
				name: "LastModify",
				table: "UserFileInfos",
				newName: "LastModefy");

			migrationBuilder.AddPrimaryKey(
				name: "PK_AppUsersSettleModefyRecord",
				table: "AppUsersSettleModefyRecord",
				column: "Code");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUsersSettleModefyRecord_AppUserSocialInfoSettles_SettleId",
				table: "AppUsersSettleModefyRecord",
				column: "SettleId",
				principalTable: "AppUserSocialInfoSettles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}