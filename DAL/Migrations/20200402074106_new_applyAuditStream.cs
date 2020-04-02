using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class new_applyAuditStream : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyResponses_Companies_CompanyCode",
                table: "ApplyResponses");

            migrationBuilder.DropIndex(
                name: "IX_ApplyResponses_CompanyCode",
                table: "ApplyResponses");

            migrationBuilder.DropColumn(
                name: "AuditLevelNum",
                table: "DutyTypes");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "ApplyResponses");

            migrationBuilder.DropColumn(
                name: "FinnalAuditCompany",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "NowAuditCompany",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "NowAuditCompanyName",
                table: "Applies");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamId",
                table: "DutyTypes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StepIndex",
                table: "ApplyResponses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NowAuditStepId",
                table: "Applies",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplyAuditSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    RequireMembersAcceptCount = table.Column<int>(nullable: false),
                    ApplyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyAuditSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyAuditSteps_Applies_ApplyId",
                        column: x => x.ApplyId,
                        principalTable: "Applies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplyAuditStreams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyAuditStreams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplyAuditStreamNodeActions",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<Guid>(nullable: false),
                    DutyIsMajor = table.Column<int>(nullable: false),
                    CompanyRefer = table.Column<string>(nullable: true),
                    AuditMembersCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyAuditStreamNodeActions", x => x.Code);
                    table.ForeignKey(
                        name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_Id",
                        column: x => x.Id,
                        principalTable: "ApplyAuditStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplyAuditStreamSolutionRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DutyIsMajor = table.Column<int>(nullable: false),
                    CompanyRefer = table.Column<string>(nullable: true),
                    AuditMembersCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    Enable = table.Column<bool>(nullable: false),
                    SolutionId = table.Column<Guid>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyAuditStreamSolutionRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionId",
                        column: x => x.SolutionId,
                        principalTable: "ApplyAuditStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Data = table.Column<int>(nullable: false),
                    ApplyAuditStreamNodeActionCode = table.Column<int>(nullable: true),
                    ApplyAuditStreamSolutionRuleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode",
                        column: x => x.ApplyAuditStreamNodeActionCode,
                        principalTable: "ApplyAuditStreamNodeActions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                        column: x => x.ApplyAuditStreamSolutionRuleId,
                        principalTable: "ApplyAuditStreamSolutionRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StringResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    ApplyAuditStepId = table.Column<Guid>(nullable: true),
                    ApplyAuditStepId1 = table.Column<Guid>(nullable: true),
                    ApplyAuditStreamNodeActionCode = table.Column<int>(nullable: true),
                    ApplyAuditStreamNodeActionCode1 = table.Column<int>(nullable: true),
                    ApplyAuditStreamSolutionRuleId = table.Column<Guid>(nullable: true),
                    ApplyAuditStreamSolutionRuleId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StringResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditSteps_ApplyAuditStepId",
                        column: x => x.ApplyAuditStepId,
                        principalTable: "ApplyAuditSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditSteps_ApplyAuditStepId1",
                        column: x => x.ApplyAuditStepId1,
                        principalTable: "ApplyAuditSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode",
                        column: x => x.ApplyAuditStreamNodeActionCode,
                        principalTable: "ApplyAuditStreamNodeActions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode1",
                        column: x => x.ApplyAuditStreamNodeActionCode1,
                        principalTable: "ApplyAuditStreamNodeActions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                        column: x => x.ApplyAuditStreamSolutionRuleId,
                        principalTable: "ApplyAuditStreamSolutionRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId1",
                        column: x => x.ApplyAuditStreamSolutionRuleId1,
                        principalTable: "ApplyAuditStreamSolutionRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DutyTypes_ApplyAuditStreamId",
                table: "DutyTypes",
                column: "ApplyAuditStreamId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_ApplyAuditStreamSolutionRuleId",
                table: "Applies",
                column: "ApplyAuditStreamSolutionRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_NowAuditStepId",
                table: "Applies",
                column: "NowAuditStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditSteps_ApplyId",
                table: "ApplyAuditSteps",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamNodeActions_Id",
                table: "ApplyAuditStreamNodeActions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionId",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_IntResources_ApplyAuditStreamNodeActionCode",
                table: "IntResources",
                column: "ApplyAuditStreamNodeActionCode");

            migrationBuilder.CreateIndex(
                name: "IX_IntResources_ApplyAuditStreamSolutionRuleId",
                table: "IntResources",
                column: "ApplyAuditStreamSolutionRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStepId",
                table: "StringResources",
                column: "ApplyAuditStepId");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStepId1",
                table: "StringResources",
                column: "ApplyAuditStepId1");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStreamNodeActionCode",
                table: "StringResources",
                column: "ApplyAuditStreamNodeActionCode");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStreamNodeActionCode1",
                table: "StringResources",
                column: "ApplyAuditStreamNodeActionCode1");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStreamSolutionRuleId",
                table: "StringResources",
                column: "ApplyAuditStreamSolutionRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStreamSolutionRuleId1",
                table: "StringResources",
                column: "ApplyAuditStreamSolutionRuleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                table: "Applies",
                column: "ApplyAuditStreamSolutionRuleId",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyAuditSteps_NowAuditStepId",
                table: "Applies",
                column: "NowAuditStepId",
                principalTable: "ApplyAuditSteps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DutyTypes_ApplyAuditStreams_ApplyAuditStreamId",
                table: "DutyTypes",
                column: "ApplyAuditStreamId",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyAuditSteps_NowAuditStepId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_DutyTypes_ApplyAuditStreams_ApplyAuditStreamId",
                table: "DutyTypes");

            migrationBuilder.DropTable(
                name: "IntResources");

            migrationBuilder.DropTable(
                name: "StringResources");

            migrationBuilder.DropTable(
                name: "ApplyAuditSteps");

            migrationBuilder.DropTable(
                name: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropTable(
                name: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropTable(
                name: "ApplyAuditStreams");

            migrationBuilder.DropIndex(
                name: "IX_DutyTypes_ApplyAuditStreamId",
                table: "DutyTypes");

            migrationBuilder.DropIndex(
                name: "IX_Applies_ApplyAuditStreamSolutionRuleId",
                table: "Applies");

            migrationBuilder.DropIndex(
                name: "IX_Applies_NowAuditStepId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamId",
                table: "DutyTypes");

            migrationBuilder.DropColumn(
                name: "StepIndex",
                table: "ApplyResponses");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "NowAuditStepId",
                table: "Applies");

            migrationBuilder.AddColumn<int>(
                name: "AuditLevelNum",
                table: "DutyTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "ApplyResponses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinnalAuditCompany",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NowAuditCompany",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NowAuditCompanyName",
                table: "Applies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponses_CompanyCode",
                table: "ApplyResponses",
                column: "CompanyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponses_Companies_CompanyCode",
                table: "ApplyResponses",
                column: "CompanyCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
