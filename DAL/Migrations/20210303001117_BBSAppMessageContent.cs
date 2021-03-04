using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class BBSAppMessageContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "BBSMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "ContentId",
                table: "BBSMessages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppMessageContent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMessageContent", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BBSMessages_ContentId",
                table: "BBSMessages",
                column: "ContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BBSMessages_AppMessageContent_ContentId",
                table: "BBSMessages",
                column: "ContentId",
                principalTable: "AppMessageContent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BBSMessages_AppMessageContent_ContentId",
                table: "BBSMessages");

            migrationBuilder.DropTable(
                name: "AppMessageContent");

            migrationBuilder.DropIndex(
                name: "IX_BBSMessages_ContentId",
                table: "BBSMessages");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "BBSMessages");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "BBSMessages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
