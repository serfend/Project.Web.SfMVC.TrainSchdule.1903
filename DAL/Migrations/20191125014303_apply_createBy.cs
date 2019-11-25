using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class apply_createBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateById",
                table: "ApplyBaseInfos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_CreateById",
                table: "ApplyBaseInfos",
                column: "CreateById");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyBaseInfos_AppUsers_CreateById",
                table: "ApplyBaseInfos",
                column: "CreateById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyBaseInfos_AppUsers_CreateById",
                table: "ApplyBaseInfos");

            migrationBuilder.DropIndex(
                name: "IX_ApplyBaseInfos_CreateById",
                table: "ApplyBaseInfos");

            migrationBuilder.DropColumn(
                name: "CreateById",
                table: "ApplyBaseInfos");
        }
    }
}
