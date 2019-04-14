using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainSchdule.DAL.Migrations
{
    public partial class AddUserDetail_ForVocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DutiesId",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Duties",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_DutiesId",
                table: "AppUsers",
                column: "DutiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Duties_DutiesId",
                table: "AppUsers",
                column: "DutiesId",
                principalTable: "Duties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Duties_DutiesId",
                table: "AppUsers");

            migrationBuilder.DropTable(
                name: "Duties");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_DutiesId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DutiesId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "AppUsers");
        }
    }
}
