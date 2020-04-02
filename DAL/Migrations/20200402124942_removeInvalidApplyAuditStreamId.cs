using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class removeInvalidApplyAuditStreamId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreams_ApplyAuditStreamSolutionRules_Name",
                table: "ApplyAuditStreams");

            migrationBuilder.DropForeignKey(
                name: "FK_DutyTypes_ApplyAuditStreams_ApplyAuditStreamId",
                table: "DutyTypes");

            migrationBuilder.DropIndex(
                name: "IX_DutyTypes_ApplyAuditStreamId",
                table: "DutyTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreams",
                table: "ApplyAuditStreams");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreams_Name",
                table: "ApplyAuditStreams");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamNodeActions_ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamId",
                table: "DutyTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApplyAuditStreams");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DutyTypes",
                nullable: true,
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

            migrationBuilder.AddColumn<string>(
                name: "ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreams",
                table: "ApplyAuditStreams",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_DutyTypes_Name",
                table: "DutyTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionName",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionName");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditStreamNodeActions_ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions",
                column: "ApplyAuditStreamName");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions",
                column: "ApplyAuditStreamName",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionName",
                table: "ApplyAuditStreamSolutionRules",
                column: "SolutionName",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DutyTypes_ApplyAuditStreams_Name",
                table: "DutyTypes",
                column: "Name",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreamNodeActions_ApplyAuditStreams_ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditStreamSolutionRules_ApplyAuditStreams_SolutionName",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropForeignKey(
                name: "FK_DutyTypes_ApplyAuditStreams_Name",
                table: "DutyTypes");

            migrationBuilder.DropIndex(
                name: "IX_DutyTypes_Name",
                table: "DutyTypes");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamSolutionRules_SolutionName",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyAuditStreams",
                table: "ApplyAuditStreams");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditStreamNodeActions_ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "SolutionName",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "ApplyAuditStreamName",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "DutyTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamId",
                table: "DutyTypes",
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

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyAuditStreamId",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyAuditStreams",
                table: "ApplyAuditStreams",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DutyTypes_ApplyAuditStreamId",
                table: "DutyTypes",
                column: "ApplyAuditStreamId");

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
                name: "FK_DutyTypes_ApplyAuditStreams_ApplyAuditStreamId",
                table: "DutyTypes",
                column: "ApplyAuditStreamId",
                principalTable: "ApplyAuditStreams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
