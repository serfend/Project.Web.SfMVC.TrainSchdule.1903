using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AppMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "BBSMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppUserRelates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Relation = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRelates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserRelates_AppUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUserRelates_AppUsers_ToId",
                        column: x => x.ToId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAppMessageeInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Setting = table.Column<int>(type: "int", nullable: false),
                    FansCount = table.Column<int>(type: "int", nullable: false),
                    FollowCount = table.Column<int>(type: "int", nullable: false),
                    UnreadMessage = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAppMessageeInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAppMessageeInfos_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRelates_FromId",
                table: "AppUserRelates",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRelates_ToId",
                table: "AppUserRelates",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAppMessageeInfos_UserId",
                table: "UserAppMessageeInfos",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserRelates");

            migrationBuilder.DropTable(
                name: "UserAppMessageeInfos");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "BBSMessages");
        }
    }
}
