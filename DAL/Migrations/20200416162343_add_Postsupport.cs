using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class add_Postsupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    ContentId = table.Column<Guid>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    CreateById = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLikes_PostContents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "PostContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostLikes_AppUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_ContentId",
                table: "PostLikes",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_CreateById",
                table: "PostLikes",
                column: "CreateById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostLikes");
        }
    }
}
