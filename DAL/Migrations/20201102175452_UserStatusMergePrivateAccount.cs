using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class UserStatusMergePrivateAccount : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "PrivateAccount",
				table: "AppUsers");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>(
				name: "PrivateAccount",
				table: "AppUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);
		}
	}
}