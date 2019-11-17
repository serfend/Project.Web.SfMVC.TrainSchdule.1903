using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ZX_Grade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ValueFormat = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Standard",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BelongToId = table.Column<Guid>(nullable: true),
                    FullGrade = table.Column<int>(nullable: false),
                    ExpressionWhenFullGrade = table.Column<string>(nullable: true),
                    minAge = table.Column<int>(nullable: false),
                    maxAge = table.Column<int>(nullable: false),
                    gender = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Standard_Subjects_BelongToId",
                        column: x => x.BelongToId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SingleGradePair",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    StandardId = table.Column<Guid>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Standard_BelongToId",
                table: "Standard",
                column: "BelongToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SingleGradePair");

            migrationBuilder.DropTable(
                name: "Standard");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
