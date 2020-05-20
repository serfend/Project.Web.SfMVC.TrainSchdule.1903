using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class userFile_parent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "UserFileInfos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFileInfos_ParentId",
                table: "UserFileInfos",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFileInfos_UserFileInfos_ParentId",
                table: "UserFileInfos",
                column: "ParentId",
                principalTable: "UserFileInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFileInfos_UserFileInfos_ParentId",
                table: "UserFileInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserFileInfos_ParentId",
                table: "UserFileInfos");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "UserFileInfos");
        }
    }
}
