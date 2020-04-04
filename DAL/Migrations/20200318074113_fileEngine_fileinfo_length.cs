using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
	public partial class fileEngine_fileinfo_length : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "Create",
				table: "UserFileInfos",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<long>(
				name: "Length",
				table: "UserFileInfos",
				nullable: false,
				defaultValue: 0L);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Create",
				table: "UserFileInfos");

			migrationBuilder.DropColumn(
				name: "Length",
				table: "UserFileInfos");
		}
	}
}