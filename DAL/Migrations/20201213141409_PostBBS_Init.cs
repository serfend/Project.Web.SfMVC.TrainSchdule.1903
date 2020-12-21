using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PostBBS_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "PostContents",
                newName: "ReplyCount");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "PostContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PostInteracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostStatus = table.Column<int>(type: "int", nullable: false),
                    CreateById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostInteracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostInteracts_AppUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostInteracts_PostContents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "PostContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostInteracts_ContentId",
                table: "PostInteracts",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_PostInteracts_CreateById",
                table: "PostInteracts",
                column: "CreateById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostInteracts");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "PostContents");

            migrationBuilder.RenameColumn(
                name: "ReplyCount",
                table: "PostContents",
                newName: "ViewCount");

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostLikes_AppUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostLikes_PostContents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "PostContents",
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
    }
}
