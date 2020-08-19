using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class CompanyAndUserStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "Applies");

            migrationBuilder.AddColumn<int>(
                name: "CompanyStatus",
                table: "Companies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LocationCode",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppUserSocialInfos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_LocationCode",
                table: "Companies",
                column: "LocationCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_AdminDivisions_LocationCode",
                table: "Companies",
                column: "LocationCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_AdminDivisions_LocationCode",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_LocationCode",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanyStatus",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppUserSocialInfos");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "Applies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
