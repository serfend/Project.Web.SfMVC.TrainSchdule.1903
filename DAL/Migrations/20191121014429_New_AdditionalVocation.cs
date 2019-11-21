using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class New_AdditionalVocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VocationAdditionals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Length = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ApplyRequestId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VocationAdditionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VocationAdditionals_ApplyRequests_ApplyRequestId",
                        column: x => x.ApplyRequestId,
                        principalTable: "ApplyRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VocationAdditionals_ApplyRequestId",
                table: "VocationAdditionals",
                column: "ApplyRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VocationAdditionals");
        }
    }
}
