using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class GradePhyExam2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateById",
                table: "GradePhyRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "GradePhyRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GradeExams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoldByCode",
                table: "GradeExams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GradePhyRecords_CreateById",
                table: "GradePhyRecords",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_GradeExams_HoldByCode",
                table: "GradeExams",
                column: "HoldByCode");

            migrationBuilder.AddForeignKey(
                name: "FK_GradeExams_Companies_HoldByCode",
                table: "GradeExams",
                column: "HoldByCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GradePhyRecords_AppUsers_CreateById",
                table: "GradePhyRecords",
                column: "CreateById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradeExams_Companies_HoldByCode",
                table: "GradeExams");

            migrationBuilder.DropForeignKey(
                name: "FK_GradePhyRecords_AppUsers_CreateById",
                table: "GradePhyRecords");

            migrationBuilder.DropIndex(
                name: "IX_GradePhyRecords_CreateById",
                table: "GradePhyRecords");

            migrationBuilder.DropIndex(
                name: "IX_GradeExams_HoldByCode",
                table: "GradeExams");

            migrationBuilder.DropColumn(
                name: "CreateById",
                table: "GradePhyRecords");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "GradePhyRecords");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GradeExams");

            migrationBuilder.DropColumn(
                name: "HoldByCode",
                table: "GradeExams");
        }
    }
}
