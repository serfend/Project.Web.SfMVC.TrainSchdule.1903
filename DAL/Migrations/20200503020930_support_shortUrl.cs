using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class support_shortUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VacationDescription",
                table: "ApplyRequests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommonShortUrl",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    Target = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    Expire = table.Column<DateTime>(nullable: false),
                    CreateById = table.Column<string>(nullable: true),
                    Ip = table.Column<string>(nullable: true),
                    Device = table.Column<string>(nullable: true),
                    UA = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonShortUrl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonShortUrl_AppUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommonShortUrlStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    UrlId = table.Column<Guid>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    Device = table.Column<string>(nullable: true),
                    UA = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonShortUrlStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonShortUrlStatistics_CommonShortUrl_UrlId",
                        column: x => x.UrlId,
                        principalTable: "CommonShortUrl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonShortUrl_CreateById",
                table: "CommonShortUrl",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_CommonShortUrl_Key",
                table: "CommonShortUrl",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_CommonShortUrlStatistics_UrlId",
                table: "CommonShortUrlStatistics",
                column: "UrlId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonShortUrlStatistics");

            migrationBuilder.DropTable(
                name: "CommonShortUrl");

            migrationBuilder.DropColumn(
                name: "VacationDescription",
                table: "ApplyRequests");
        }
    }
}
