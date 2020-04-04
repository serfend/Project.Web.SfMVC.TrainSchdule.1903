using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
	public partial class recall_allownull : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<Guid>(
				name: "RecallId",
				table: "Applies",
				nullable: true,
				oldClrType: typeof(Guid));
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<Guid>(
				name: "RecallId",
				table: "Applies",
				nullable: false,
				oldClrType: typeof(Guid),
				oldNullable: true);
		}
	}
}