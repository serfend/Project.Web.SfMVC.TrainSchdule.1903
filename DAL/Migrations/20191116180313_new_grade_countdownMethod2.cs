using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class new_grade_countdownMethod2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standard_Subjects_SubjectId",
                table: "Standard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Standard",
                table: "Standard");

            migrationBuilder.RenameTable(
                name: "Standard",
                newName: "Standards");

            migrationBuilder.RenameIndex(
                name: "IX_Standard_SubjectId",
                table: "Standards",
                newName: "IX_Standards_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Standards",
                table: "Standards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Standards_Subjects_SubjectId",
                table: "Standards",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standards_Subjects_SubjectId",
                table: "Standards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Standards",
                table: "Standards");

            migrationBuilder.RenameTable(
                name: "Standards",
                newName: "Standard");

            migrationBuilder.RenameIndex(
                name: "IX_Standards_SubjectId",
                table: "Standard",
                newName: "IX_Standard_SubjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Standard",
                table: "Standard",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Standard_Subjects_SubjectId",
                table: "Standard",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
