using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class subject_rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standards_Subjects_SubjectId",
                table: "Standards");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Standards",
                newName: "GradePhySubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Standards_SubjectId",
                table: "Standards",
                newName: "IX_Standards_GradePhySubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Standards_Subjects_GradePhySubjectId",
                table: "Standards",
                column: "GradePhySubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standards_Subjects_GradePhySubjectId",
                table: "Standards");

            migrationBuilder.RenameColumn(
                name: "GradePhySubjectId",
                table: "Standards",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Standards_GradePhySubjectId",
                table: "Standards",
                newName: "IX_Standards_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Standards_Subjects_SubjectId",
                table: "Standards",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
