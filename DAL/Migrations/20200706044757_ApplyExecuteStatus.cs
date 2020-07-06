using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ApplyExecuteStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecallOrders_AppUsers_RecallById",
                table: "RecallOrders");

            migrationBuilder.RenameColumn(
                name: "RecallById",
                table: "RecallOrders",
                newName: "HandleById");

            migrationBuilder.RenameIndex(
                name: "IX_RecallOrders_RecallById",
                table: "RecallOrders",
                newName: "IX_RecallOrders_HandleById");

            migrationBuilder.AddColumn<int>(
                name: "ExecuteStatus",
                table: "Applies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ExecuteStatusDetailId",
                table: "Applies",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplyExcuteStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    HandleById = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    ReturnStramp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyExcuteStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyExcuteStatus_AppUsers_HandleById",
                        column: x => x.HandleById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applies_ExecuteStatusDetailId",
                table: "Applies",
                column: "ExecuteStatusDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyExcuteStatus_HandleById",
                table: "ApplyExcuteStatus",
                column: "HandleById");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyExcuteStatus_ExecuteStatusDetailId",
                table: "Applies",
                column: "ExecuteStatusDetailId",
                principalTable: "ApplyExcuteStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecallOrders_AppUsers_HandleById",
                table: "RecallOrders",
                column: "HandleById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyExcuteStatus_ExecuteStatusDetailId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_RecallOrders_AppUsers_HandleById",
                table: "RecallOrders");

            migrationBuilder.DropTable(
                name: "ApplyExcuteStatus");

            migrationBuilder.DropIndex(
                name: "IX_Applies_ExecuteStatusDetailId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "ExecuteStatus",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "ExecuteStatusDetailId",
                table: "Applies");

            migrationBuilder.RenameColumn(
                name: "HandleById",
                table: "RecallOrders",
                newName: "RecallById");

            migrationBuilder.RenameIndex(
                name: "IX_RecallOrders_HandleById",
                table: "RecallOrders",
                newName: "IX_RecallOrders_RecallById");

            migrationBuilder.AddForeignKey(
                name: "FK_RecallOrders_AppUsers_RecallById",
                table: "RecallOrders",
                column: "RecallById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
