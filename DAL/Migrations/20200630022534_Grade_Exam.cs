using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Grade_Exam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradeExams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreateById = table.Column<string>(nullable: true),
                    HandleById = table.Column<string>(nullable: true),
                    ExecuteTime = table.Column<DateTime>(nullable: false),
                    Create = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeExams_AppUsers_CreateById",
                        column: x => x.CreateById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeExams_AppUsers_HandleById",
                        column: x => x.HandleById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GradePhyRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: true),
                    ExamId = table.Column<int>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    RawValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradePhyRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradePhyRecords_GradeExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "GradeExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradePhyRecords_GradePhySubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "GradePhySubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradePhyRecords_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeExams_CreateById",
                table: "GradeExams",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_GradeExams_HandleById",
                table: "GradeExams",
                column: "HandleById");

            migrationBuilder.CreateIndex(
                name: "IX_GradePhyRecords_ExamId",
                table: "GradePhyRecords",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_GradePhyRecords_SubjectId",
                table: "GradePhyRecords",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GradePhyRecords_UserId",
                table: "GradePhyRecords",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradePhyRecords");

            migrationBuilder.DropTable(
                name: "GradeExams");
        }
    }
}
