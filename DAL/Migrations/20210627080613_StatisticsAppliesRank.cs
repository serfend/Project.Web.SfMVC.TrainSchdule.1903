using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class StatisticsAppliesRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyIndayRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyIndayRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyRequests");

            migrationBuilder.AlterColumn<int>(
                name: "VacationPlaceCode",
                table: "ApplyRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VacationPlaceCode",
                table: "ApplyIndayRequests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "StatisticsApplyRanks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Target = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RatingCycleCount = table.Column<int>(type: "int", nullable: false),
                    RatingType = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinnalResult = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsApplyRanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticsApplyRanks_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatisticsApplyRanks_UserId",
                table: "StatisticsApplyRanks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyIndayRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyIndayRequests",
                column: "VacationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyRequests",
                column: "VacationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyIndayRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyIndayRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyRequests");

            migrationBuilder.DropTable(
                name: "StatisticsApplyRanks");

            migrationBuilder.AlterColumn<int>(
                name: "VacationPlaceCode",
                table: "ApplyRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "VacationPlaceCode",
                table: "ApplyIndayRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyIndayRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyIndayRequests",
                column: "VacationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyRequests",
                column: "VacationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
