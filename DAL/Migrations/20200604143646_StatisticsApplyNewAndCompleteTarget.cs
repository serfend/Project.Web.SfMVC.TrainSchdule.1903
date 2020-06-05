using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class StatisticsApplyNewAndCompleteTarget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Duties",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StatisticsCompleteApplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyCode = table.Column<string>(nullable: true),
                    To = table.Column<byte>(nullable: false),
                    From = table.Column<byte>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Target = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsCompleteApplies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatisticsNewApplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyCode = table.Column<string>(nullable: true),
                    To = table.Column<byte>(nullable: false),
                    From = table.Column<byte>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Target = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticsNewApplies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatisticsCompleteApplies");

            migrationBuilder.DropTable(
                name: "StatisticsNewApplies");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Duties");
        }
    }
}
