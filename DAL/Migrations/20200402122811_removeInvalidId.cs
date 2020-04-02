using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class removeInvalidId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_Id",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionId",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropForeignKey(
                name: "FK_IntResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode",
                table: "IntResources");

            migrationBuilder.DropForeignKey(
                name: "FK_IntResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                table: "IntResources");

            migrationBuilder.DropForeignKey(
                name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode",
                table: "StringResources");

            migrationBuilder.DropForeignKey(
                name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode1",
                table: "StringResources");

            migrationBuilder.DropForeignKey(
                name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                table: "StringResources");

            migrationBuilder.DropForeignKey(
                name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId1",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_StringResources_ApplyAuditStreamNodeActionCode",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_StringResources_ApplyAuditStreamNodeActionCode1",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_StringResources_ApplyAuditStreamSolutionRuleId",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_StringResources_ApplyAuditStreamSolutionRuleId1",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_IntResources_ApplyAuditStreamNodeActionCode",
                table: "IntResources");

            migrationBuilder.DropIndex(
                name: "IX_IntResources_ApplyAuditStreamSolutionRuleId",
                table: "IntResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreamSolutionRules",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionId",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreamNodeActions",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamNodeActions_Id",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropIndex(
                name: "IX_Applies_ApplyAuditStreamSolutionRuleId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamNodeActionCode",
                table: "StringResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamNodeActionCode1",
                table: "StringResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "StringResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleId1",
                table: "StringResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamNodeActionCode",
                table: "IntResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "IntResources");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "SolutionId",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "Applies");

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamNodeActionName",
                table: "StringResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamNodeActionName1",
                table: "StringResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamSolutionRuleName",
                table: "StringResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamSolutionRuleName1",
                table: "StringResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamNodeActionName",
                table: "IntResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamSolutionRuleName",
                table: "IntResources",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreamSolutionRules",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreams",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreamNodeActions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamSolutionRuleName",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreamSolutionRules",
                table: "ApplyAuditStreamSolutionRules",
                column: "Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreamNodeActions",
                table: "ApplyAuditStreamNodeActions",
                column: "Name");

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

            migrationBuilder.CreateIndex(
                name: "IX_IntResources_ApplyAuditStreamNodeActionName",
                table: "IntResources",
                column: "ApplyAuditStreamNodeActionName");

            migrationBuilder.CreateIndex(
                name: "IX_IntResources_ApplyAuditStreamSolutionRuleName",
                table: "IntResources",
                column: "ApplyAuditStreamSolutionRuleName");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreams_Name",
                table: "ApplyAuditStreams",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamNodeActions_ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions",
                column: "ApplyAuditStreamId");

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
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions",
                column: "ApplyAuditStreamId",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyAuditStreams_ApplyAuditStreamSolutionRules_Name",
                table: "ApplyAuditStreams",
                column: "Name",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IntResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName",
                table: "IntResources",
                column: "ApplyAuditStreamNodeActionName",
                principalTable: "ApplyAuditStreamNodeActions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IntResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                table: "IntResources",
                column: "ApplyAuditStreamSolutionRuleName",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName",
                table: "StringResources",
                column: "ApplyAuditStreamNodeActionName",
                principalTable: "ApplyAuditStreamNodeActions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName1",
                table: "StringResources",
                column: "ApplyAuditStreamNodeActionName1",
                principalTable: "ApplyAuditStreamNodeActions",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                table: "StringResources",
                column: "ApplyAuditStreamSolutionRuleName",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName1",
                table: "StringResources",
                column: "ApplyAuditStreamSolutionRuleName1",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreams_ApplyAuditStreamSolutionRules_Name",
                table: "ApplyAuditStreams");

            migrationBuilder.DropForeignKey(
                name: "FK_IntResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName",
                table: "IntResources");

            migrationBuilder.DropForeignKey(
                name: "FK_IntResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                table: "IntResources");

            migrationBuilder.DropForeignKey(
                name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName",
                table: "StringResources");

            migrationBuilder.DropForeignKey(
                name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionName1",
                table: "StringResources");

            migrationBuilder.DropForeignKey(
                name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName",
                table: "StringResources");

            migrationBuilder.DropForeignKey(
                name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleName1",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_StringResources_ApplyAuditStreamNodeActionName",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_StringResources_ApplyAuditStreamNodeActionName1",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_StringResources_ApplyAuditStreamSolutionRuleName",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_StringResources_ApplyAuditStreamSolutionRuleName1",
                table: "StringResources");

            migrationBuilder.DropIndex(
                name: "IX_IntResources_ApplyAuditStreamNodeActionName",
                table: "IntResources");

            migrationBuilder.DropIndex(
                name: "IX_IntResources_ApplyAuditStreamSolutionRuleName",
                table: "IntResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreamSolutionRules",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreams_Name",
                table: "ApplyAuditStreams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreamNodeActions",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamNodeActions_ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropIndex(
                name: "IX_Applies_ApplyAuditStreamSolutionRuleName",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamNodeActionName",
                table: "StringResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamNodeActionName1",
                table: "StringResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleName",
                table: "StringResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleName1",
                table: "StringResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamNodeActionName",
                table: "IntResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleName",
                table: "IntResources");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamSolutionRuleName",
                table: "Applies");

            migrationBuilder.AddColumn<int>(
                name: "ApplyAuditStreamNodeActionCode",
                table: "StringResources",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplyAuditStreamNodeActionCode1",
                table: "StringResources",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "StringResources",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamSolutionRuleId1",
                table: "StringResources",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplyAuditStreamNodeActionCode",
                table: "IntResources",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamSolutionRuleId",
                table: "IntResources",
                nullable: true);

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
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ApplyAuditStreamNodeActions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "ApplyAuditStreamNodeActions",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                name: "PK_ApplyAuditStreamNodeActions",
                table: "ApplyAuditStreamNodeActions",
                column: "Code");

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

            migrationBuilder.CreateIndex(
                name: "IX_IntResources_ApplyAuditStreamNodeActionCode",
                table: "IntResources",
                column: "ApplyAuditStreamNodeActionCode");

            migrationBuilder.CreateIndex(
                name: "IX_IntResources_ApplyAuditStreamSolutionRuleId",
                table: "IntResources",
                column: "ApplyAuditStreamSolutionRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionId",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamNodeActions_Id",
                table: "ApplyAuditStreamNodeActions",
                column: "Id");

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
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_Id",
                table: "ApplyAuditStreamNodeActions",
                column: "Id",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionId",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionId",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IntResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode",
                table: "IntResources",
                column: "ApplyAuditStreamNodeActionCode",
                principalTable: "ApplyAuditStreamNodeActions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IntResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                table: "IntResources",
                column: "ApplyAuditStreamSolutionRuleId",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode",
                table: "StringResources",
                column: "ApplyAuditStreamNodeActionCode",
                principalTable: "ApplyAuditStreamNodeActions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StringResources_ApplyAuditStreamNodeActions_ApplyAuditStreamNodeActionCode1",
                table: "StringResources",
                column: "ApplyAuditStreamNodeActionCode1",
                principalTable: "ApplyAuditStreamNodeActions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                table: "StringResources",
                column: "ApplyAuditStreamSolutionRuleId",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StringResources_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId1",
                table: "StringResources",
                column: "ApplyAuditStreamSolutionRuleId1",
                principalTable: "ApplyAuditStreamSolutionRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
