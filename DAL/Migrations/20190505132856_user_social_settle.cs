using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class user_social_settle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Settle",
                table: "UserSocialInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settle",
                table: "UserSocialInfo");
        }
    }
}
