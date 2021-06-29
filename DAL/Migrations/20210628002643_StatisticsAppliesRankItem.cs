using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class StatisticsAppliesRankItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "StatisticsApplyRanks");

            migrationBuilder.DropColumn(
                name: "FinnalResult",
                table: "StatisticsApplyRanks");

            migrationBuilder.DropColumn(
                name: "LastUpdate",
                table: "StatisticsApplyRanks");

            migrationBuilder.CreateTable(
                name: "StatisticsApplyRankRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinnalResult = table.Column<bool>(type: "bit", nullable: false),
                    ApplyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Target = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RatingCycleCount = table.Column<int>(type: "int", nullable: false),
                    RatingType = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsApplyRankRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticsApplyRankRecords_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsApplyRankRecords_UserId",
                table: "StatisticsApplyRankRecords",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticsApplyRankRecords");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "StatisticsApplyRanks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "FinnalResult",
                table: "StatisticsApplyRanks",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdate",
                table: "StatisticsApplyRanks",
                type: "datetime2",
                nullable: true);
        }
    }
}
