using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ClientTagListHandle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientTags_Clients_ClientId",
                table: "ClientTags");

            migrationBuilder.DropIndex(
                name: "IX_ClientTags_ClientId",
                table: "ClientTags");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "ClientTags");

            migrationBuilder.CreateTable(
                name: "ClientWithTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientWithTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientWithTags_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientWithTags_ClientTags_ClientTagsId",
                        column: x => x.ClientTagsId,
                        principalTable: "ClientTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientWithTags_ClientId",
                table: "ClientWithTags",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientWithTags_ClientTagsId",
                table: "ClientWithTags",
                column: "ClientTagsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientWithTags");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "ClientTags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientTags_ClientId",
                table: "ClientTags",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientTags_Clients_ClientId",
                table: "ClientTags",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
