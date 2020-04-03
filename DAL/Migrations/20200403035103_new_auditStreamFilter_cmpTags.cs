using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class new_auditStreamFilter_cmpTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DutyTypes");

            migrationBuilder.DropColumn(
                name: "DutiesRawType",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Duties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCodeLength",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTags",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DutiesTags",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCodeLength",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTags",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DutiesTags",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanyCodeLength",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "CompanyTags",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "DutiesTags",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "CompanyCodeLength",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "CompanyTags",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "DutiesTags",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.AddColumn<int>(
                name: "DutiesRawType",
                table: "Duties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Companies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DutyTypes",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DutiesCode = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NotCaculateStatistics = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyTypes", x => x.Code);
                    table.ForeignKey(
                        name: "FK_DutyTypes_Duties_DutiesCode",
                        column: x => x.DutiesCode,
                        principalTable: "Duties",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DutyTypes_ApplyAuditStreams_Name",
                        column: x => x.Name,
                        principalTable: "ApplyAuditStreams",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DutyTypes_DutiesCode",
                table: "DutyTypes",
                column: "DutiesCode");

            migrationBuilder.CreateIndex(
                name: "IX_DutyTypes_Name",
                table: "DutyTypes",
                column: "Name");
        }
    }
}
