using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Companies_ParentCode",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ParentCode",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ParentCode",
                table: "Companies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentCode",
                table: "Companies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ParentCode",
                table: "Companies",
                column: "ParentCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Companies_ParentCode",
                table: "Companies",
                column: "ParentCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
