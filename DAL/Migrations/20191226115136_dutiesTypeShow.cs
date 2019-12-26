using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class dutiesTypeShow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DutiesRawType",
                table: "DutyTypes");

            migrationBuilder.AddColumn<int>(
                name: "DutiesRawType",
                table: "Duties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "XlsTempletes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    CreateById = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XlsTempletes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XlsTempletes_AppUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_XlsTempletes_CreateById",
                table: "XlsTempletes",
                column: "CreateById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "XlsTempletes");

            migrationBuilder.DropColumn(
                name: "DutiesRawType",
                table: "Duties");

            migrationBuilder.AddColumn<int>(
                name: "DutiesRawType",
                table: "DutyTypes",
                nullable: false,
                defaultValue: 0);
        }
    }
}
