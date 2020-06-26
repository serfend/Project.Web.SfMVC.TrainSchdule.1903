using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class subject_rename2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standards_Subjects_GradePhySubjectId",
                table: "Standards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Standards",
                table: "Standards");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "GradePhySubjects");

            migrationBuilder.RenameTable(
                name: "Standards",
                newName: "GradePhyStandards");

            migrationBuilder.RenameIndex(
                name: "IX_Standards_GradePhySubjectId",
                table: "GradePhyStandards",
                newName: "IX_GradePhyStandards_GradePhySubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradePhySubjects",
                table: "GradePhySubjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradePhyStandards",
                table: "GradePhyStandards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradePhyStandards_GradePhySubjects_GradePhySubjectId",
                table: "GradePhyStandards",
                column: "GradePhySubjectId",
                principalTable: "GradePhySubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradePhyStandards_GradePhySubjects_GradePhySubjectId",
                table: "GradePhyStandards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradePhySubjects",
                table: "GradePhySubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradePhyStandards",
                table: "GradePhyStandards");

            migrationBuilder.RenameTable(
                name: "GradePhySubjects",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "GradePhyStandards",
                newName: "Standards");

            migrationBuilder.RenameIndex(
                name: "IX_GradePhyStandards_GradePhySubjectId",
                table: "Standards",
                newName: "IX_Standards_GradePhySubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Standards",
                table: "Standards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Standards_Subjects_GradePhySubjectId",
                table: "Standards",
                column: "GradePhySubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
