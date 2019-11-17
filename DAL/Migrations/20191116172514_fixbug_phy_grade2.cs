using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fixbug_phy_grade2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standard_Subjects_BelongToId",
                table: "Standard");

            migrationBuilder.RenameColumn(
                name: "BelongToId",
                table: "Standard",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Standard_BelongToId",
                table: "Standard",
                newName: "IX_Standard_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Standard_Subjects_SubjectId",
                table: "Standard",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Standard_Subjects_SubjectId",
                table: "Standard");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Standard",
                newName: "BelongToId");

            migrationBuilder.RenameIndex(
                name: "IX_Standard_SubjectId",
                table: "Standard",
                newName: "IX_Standard_BelongToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Standard_Subjects_BelongToId",
                table: "Standard",
                column: "BelongToId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
