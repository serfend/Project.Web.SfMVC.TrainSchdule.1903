using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class applyStream_addId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionName",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreamSolutionRules",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionName",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreams",
                table: "ApplyAuditStreams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreamNodeActions",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropIndex(
                name: "IX_Applies_ApplyAuditStreamSolutionRuleName",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "SolutionName",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleName",
                table: "Applies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ApplyAuditStreamSolutionRules",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SolutionId",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreams",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ApplyAuditStreams",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreamNodeActions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ApplyAuditStreamNodeActions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreamSolutionRules",
                table: "ApplyAuditStreamSolutionRules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreams",
                table: "ApplyAuditStreams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreamNodeActions",
                table: "ApplyAuditStreamNodeActions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionId",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_ApplyAuditStreamSolutionRuleId",
                table: "Applies",
                column: "ApplyAuditStreamSolutionRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                table: "Applies",
                column: "ApplyAuditStreamSolutionRuleId",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionId",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionId",
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
                name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionId",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreamSolutionRules",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionId",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreams",
                table: "ApplyAuditStreams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreamNodeActions",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropIndex(
                name: "IX_Applies_ApplyAuditStreamSolutionRuleId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "SolutionId",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApplyAuditStreams");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "Applies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreamSolutionRules",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SolutionName",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreams",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreamNodeActions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamSolutionRuleName",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreamSolutionRules",
                table: "ApplyAuditStreamSolutionRules",
                column: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreams",
                table: "ApplyAuditStreams",
                column: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreamNodeActions",
                table: "ApplyAuditStreamNodeActions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionName",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionName");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_ApplyAuditStreamSolutionRuleName",
                table: "Applies",
                column: "ApplyAuditStreamSolutionRuleName");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                table: "Applies",
                column: "ApplyAuditStreamSolutionRuleName",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionName",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionName",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
