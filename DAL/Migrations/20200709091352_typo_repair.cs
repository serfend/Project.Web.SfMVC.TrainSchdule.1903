using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class typo_repair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VocationAdditionals_ApplyRequests_ApplyRequestId",
                table: "VocationAdditionals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VocationDescriptions",
                table: "VocationDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VocationAdditionals",
                table: "VocationAdditionals");

            migrationBuilder.RenameTable(
                name: "VocationDescriptions",
                newName: "VacationDescriptions");

            migrationBuilder.RenameTable(
                name: "VocationAdditionals",
                newName: "VacationAdditionals");

            migrationBuilder.RenameColumn(
                name: "ReturnStramp",
                table: "RecallOrders",
                newName: "ReturnStamp");

            migrationBuilder.RenameColumn(
                name: "VocationType",
                table: "ApplyRequests",
                newName: "VacationType");

            migrationBuilder.RenameColumn(
                name: "VocationPlaceName",
                table: "ApplyRequests",
                newName: "VacationPlaceName");

            migrationBuilder.RenameColumn(
                name: "VocationLength",
                table: "ApplyRequests",
                newName: "VacationLength");

            migrationBuilder.RenameColumn(
                name: "ReturnStramp",
                table: "ApplyExcuteStatus",
                newName: "ReturnStamp");

            migrationBuilder.RenameIndex(
                name: "IX_VocationAdditionals_ApplyRequestId",
                table: "VacationAdditionals",
                newName: "IX_VacationAdditionals_ApplyRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationDescriptions",
                table: "VacationDescriptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VacationAdditionals",
                table: "VacationAdditionals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VacationAdditionals_ApplyRequests_ApplyRequestId",
                table: "VacationAdditionals",
                column: "ApplyRequestId",
                principalTable: "ApplyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VacationAdditionals_ApplyRequests_ApplyRequestId",
                table: "VacationAdditionals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationDescriptions",
                table: "VacationDescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VacationAdditionals",
                table: "VacationAdditionals");

            migrationBuilder.RenameTable(
                name: "VacationDescriptions",
                newName: "VocationDescriptions");

            migrationBuilder.RenameTable(
                name: "VacationAdditionals",
                newName: "VocationAdditionals");

            migrationBuilder.RenameColumn(
                name: "ReturnStamp",
                table: "RecallOrders",
                newName: "ReturnStramp");

            migrationBuilder.RenameColumn(
                name: "VacationType",
                table: "ApplyRequests",
                newName: "VocationType");

            migrationBuilder.RenameColumn(
                name: "VacationPlaceName",
                table: "ApplyRequests",
                newName: "VocationPlaceName");

            migrationBuilder.RenameColumn(
                name: "VacationLength",
                table: "ApplyRequests",
                newName: "VocationLength");

            migrationBuilder.RenameColumn(
                name: "ReturnStamp",
                table: "ApplyExcuteStatus",
                newName: "ReturnStramp");

            migrationBuilder.RenameIndex(
                name: "IX_VacationAdditionals_ApplyRequestId",
                table: "VocationAdditionals",
                newName: "IX_VocationAdditionals_ApplyRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VocationDescriptions",
                table: "VocationDescriptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VocationAdditionals",
                table: "VocationAdditionals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VocationAdditionals_ApplyRequests_ApplyRequestId",
                table: "VocationAdditionals",
                column: "ApplyRequestId",
                principalTable: "ApplyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
