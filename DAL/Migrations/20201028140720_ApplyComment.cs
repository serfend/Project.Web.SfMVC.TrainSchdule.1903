using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ApplyComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplyComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    FromId = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    LastModify = table.Column<DateTime>(nullable: false),
                    ModifyById = table.Column<string>(nullable: true),
                    ApplyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyComments_Applies_ApplyId",
                        column: x => x.ApplyId,
                        principalTable: "Applies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyComments_AppUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyComments_AppUsers_ModifyById",
                        column: x => x.ModifyById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyComments_ApplyId",
                table: "ApplyComments",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyComments_FromId",
                table: "ApplyComments",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyComments_ModifyById",
                table: "ApplyComments",
                column: "ModifyById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyComments");
        }
    }
}
