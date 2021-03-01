using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class VirusTrace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VirusTraces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sha1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirusTraces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VirusTypeDispatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VirusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VirusTraceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAutoDispatch = table.Column<bool>(type: "bit", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VirusTypeDispatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VirusTypeDispatches_Viruses_VirusId",
                        column: x => x.VirusId,
                        principalTable: "Viruses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VirusTypeDispatches_VirusTraces_VirusTraceId",
                        column: x => x.VirusTraceId,
                        principalTable: "VirusTraces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VirusTypeDispatches_VirusId",
                table: "VirusTypeDispatches",
                column: "VirusId");

            migrationBuilder.CreateIndex(
                name: "IX_VirusTypeDispatches_VirusTraceId",
                table: "VirusTypeDispatches",
                column: "VirusTraceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VirusTypeDispatches");

            migrationBuilder.DropTable(
                name: "VirusTraces");
        }
    }
}
