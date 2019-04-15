using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainSchdule.DAL.Migrations
{
    public partial class addApplyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplyId",
                table: "Companies",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplyRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    xjts = table.Column<int>(nullable: false),
                    ltts = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplyStamp",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ldsj = table.Column<DateTime>(nullable: false),
                    gdsj = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyStamp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromId = table.Column<Guid>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    RequestId = table.Column<Guid>(nullable: true),
                    xjlb = table.Column<string>(nullable: true),
                    stampId = table.Column<Guid>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applies_AppUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applies_ApplyRequest_RequestId",
                        column: x => x.RequestId,
                        principalTable: "ApplyRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applies_ApplyStamp_stampId",
                        column: x => x.stampId,
                        principalTable: "ApplyStamp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplyResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuditingById = table.Column<Guid>(nullable: true),
                    CompanyId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    HandleStamp = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ApplyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyResponse_Applies_ApplyId",
                        column: x => x.ApplyId,
                        principalTable: "Applies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyResponse_AppUsers_AuditingById",
                        column: x => x.AuditingById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyResponse_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ApplyId",
                table: "Companies",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_FromId",
                table: "Applies",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_RequestId",
                table: "Applies",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_stampId",
                table: "Applies",
                column: "stampId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponse_ApplyId",
                table: "ApplyResponse",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponse_AuditingById",
                table: "ApplyResponse",
                column: "AuditingById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponse_CompanyId",
                table: "ApplyResponse",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Applies_ApplyId",
                table: "Companies",
                column: "ApplyId",
                principalTable: "Applies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Applies_ApplyId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "ApplyResponse");

            migrationBuilder.DropTable(
                name: "Applies");

            migrationBuilder.DropTable(
                name: "ApplyRequest");

            migrationBuilder.DropTable(
                name: "ApplyStamp");

            migrationBuilder.DropIndex(
                name: "IX_Companies_ApplyId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ApplyId",
                table: "Companies");
        }
    }
}
