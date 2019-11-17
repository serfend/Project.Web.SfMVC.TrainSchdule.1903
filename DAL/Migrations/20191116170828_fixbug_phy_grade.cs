using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fixbug_phy_grade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SingleGradePair");

            migrationBuilder.AddColumn<string>(
                name: "GradePairs",
                table: "Standard",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GradePairs",
                table: "Standard");

            migrationBuilder.CreateTable(
                name: "SingleGradePair",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<int>(nullable: false),
                    StandardId = table.Column<Guid>(nullable: true),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleGradePair", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleGradePair_Standard_StandardId",
                        column: x => x.StandardId,
                        principalTable: "Standard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleGradePair_StandardId",
                table: "SingleGradePair",
                column: "StandardId");
        }
    }
}
