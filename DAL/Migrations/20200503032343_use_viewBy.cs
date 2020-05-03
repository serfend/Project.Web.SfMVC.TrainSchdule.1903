using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class use_viewBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ViewById",
                table: "CommonShortUrlStatistics",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommonShortUrlStatistics_ViewById",
                table: "CommonShortUrlStatistics",
                column: "ViewById");

            migrationBuilder.AddForeignKey(
                name: "FK_CommonShortUrlStatistics_AppUsers_ViewById",
                table: "CommonShortUrlStatistics",
                column: "ViewById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommonShortUrlStatistics_AppUsers_ViewById",
                table: "CommonShortUrlStatistics");

            migrationBuilder.DropIndex(
                name: "IX_CommonShortUrlStatistics_ViewById",
                table: "CommonShortUrlStatistics");

            migrationBuilder.DropColumn(
                name: "ViewById",
                table: "CommonShortUrlStatistics");
        }
    }
}
