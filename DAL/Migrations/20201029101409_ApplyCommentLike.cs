using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ApplyCommentLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplyCommentLikes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    Create = table.Column<DateTime>(nullable: false),
                    CreateById = table.Column<string>(nullable: true),
                    CommentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyCommentLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyCommentLikes_ApplyComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "ApplyComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyCommentLikes_AppUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyCommentLikes_CommentId",
                table: "ApplyCommentLikes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyCommentLikes_CreateById",
                table: "ApplyCommentLikes",
                column: "CreateById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyCommentLikes");
        }
    }
}
