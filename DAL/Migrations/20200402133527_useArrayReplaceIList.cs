using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class useArrayReplaceIList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropTable(
                name: "IntResources");

            migrationBuilder.DropTable(
                name: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamNodeActions_ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.RenameColumn(
                name: "ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions",
                newName: "Duties");

            migrationBuilder.AddColumn<string>(
                name: "AuditMembers",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Companies",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duties",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nodes",
                table: "ApplyAuditStreams",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duties",
                table: "ApplyAuditStreamNodeActions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditMembers",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Companies",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MembersAcceptToAudit",
                table: "ApplyAuditSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MembersFitToAudit",
                table: "ApplyAuditSteps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditMembers",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "Companies",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "Duties",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "Nodes",
                table: "ApplyAuditStreams");

            migrationBuilder.DropColumn(
                name: "AuditMembers",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "Companies",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "MembersAcceptToAudit",
                table: "ApplyAuditSteps");

            migrationBuilder.DropColumn(
                name: "MembersFitToAudit",
                table: "ApplyAuditSteps");

            migrationBuilder.RenameColumn(
                name: "Duties",
                table: "ApplyAuditStreamNodeActions",
                newName: "ApplyAuditStreamName");

            migrationBuilder.AlterColumn<string>(
                name: "ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IntResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplyAuditStreamNodeActionName = table.Column<string>(nullable: true),
                    ApplyAuditStreamSolutionRuleName = table.Column<string>(nullable: true),
                    Data = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName",
                        column: x => x.ApplyAuditStreamNodeActionName,
                        principalTable: "ApplyAuditStreamNodeActions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                        column: x => x.ApplyAuditStreamSolutionRuleName,
                        principalTable: "ApplyAuditStreamSolutionRules",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StringResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplyAuditStepId = table.Column<Guid>(nullable: true),
                    ApplyAuditStepId1 = table.Column<Guid>(nullable: true),
                    ApplyAuditStreamNodeActionName = table.Column<string>(nullable: true),
                    ApplyAuditStreamNodeActionName1 = table.Column<string>(nullable: true),
                    ApplyAuditStreamSolutionRuleName = table.Column<string>(nullable: true),
                    ApplyAuditStreamSolutionRuleName1 = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true)
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
                        name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName",
                        column: x => x.ApplyAuditStreamNodeActionName,
                        principalTable: "ApplyAuditStreamNodeActions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName1",
                        column: x => x.ApplyAuditStreamNodeActionName1,
                        principalTable: "ApplyAuditStreamNodeActions",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                        column: x => x.ApplyAuditStreamSolutionRuleName,
                        principalTable: "ApplyAuditStreamSolutionRules",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName1",
                        column: x => x.ApplyAuditStreamSolutionRuleName1,
                        principalTable: "ApplyAuditStreamSolutionRules",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamNodeActions_ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions",
                column: "ApplyAuditStreamName");

            migrationBuilder.CreateIndex(
                name: "IX_IntResources_ApplyAuditStreamNodeActionName",
                table: "IntResources",
                column: "ApplyAuditStreamNodeActionName");

            migrationBuilder.CreateIndex(
                name: "IX_IntResources_ApplyAuditStreamSolutionRuleName",
                table: "IntResources",
                column: "ApplyAuditStreamSolutionRuleName");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStepId",
                table: "StringResources",
                column: "ApplyAuditStepId");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStepId1",
                table: "StringResources",
                column: "ApplyAuditStepId1");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStreamNodeActionName",
                table: "StringResources",
                column: "ApplyAuditStreamNodeActionName");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStreamNodeActionName1",
                table: "StringResources",
                column: "ApplyAuditStreamNodeActionName1");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStreamSolutionRuleName",
                table: "StringResources",
                column: "ApplyAuditStreamSolutionRuleName");

            migrationBuilder.CreateIndex(
                name: "IX_StringResources_ApplyAuditStreamSolutionRuleName1",
                table: "StringResources",
                column: "ApplyAuditStreamSolutionRuleName1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions",
                column: "ApplyAuditStreamName",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
