using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PostInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyComments_Applies_ApplyId",
                table: "ApplyComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostContents_PostContents_PostId",
                table: "PostContents");

            migrationBuilder.DropIndex(
                name: "IX_PostContents_PostId",
                table: "PostContents");

            migrationBuilder.DropIndex(
                name: "IX_ApplyComments_ApplyId",
                table: "ApplyComments");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PostContents");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "PostContents");

            migrationBuilder.DropColumn(
                name: "ApplyId",
                table: "ApplyComments");

            migrationBuilder.AddColumn<string>(
                name: "Apply",
                table: "ApplyComments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apply",
                table: "ApplyComments");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PostContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "PostContents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyId",
                table: "ApplyComments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostContents_PostId",
                table: "PostContents",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyComments_ApplyId",
                table: "ApplyComments",
                column: "ApplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyComments_Applies_ApplyId",
                table: "ApplyComments",
                column: "ApplyId",
                principalTable: "Applies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostContents_PostContents_PostId",
                table: "PostContents",
                column: "PostId",
                principalTable: "PostContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
