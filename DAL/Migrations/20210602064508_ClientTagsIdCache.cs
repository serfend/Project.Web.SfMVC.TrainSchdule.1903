using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ClientTagsIdCache : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ClientTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientTags_ParentId",
                table: "ClientTags",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTags_ClientTags_ParentId",
                table: "ClientTags",
                column: "ParentId",
                principalTable: "ClientTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientTags_ClientTags_ParentId",
                table: "ClientTags");

            migrationBuilder.DropIndex(
                name: "IX_ClientTags_ParentId",
                table: "ClientTags");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ClientTags");
        }
    }
}
