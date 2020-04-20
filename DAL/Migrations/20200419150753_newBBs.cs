using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class newBBs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "PostContents",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "PostContents",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "PostContents");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "PostContents");
        }
    }
}
