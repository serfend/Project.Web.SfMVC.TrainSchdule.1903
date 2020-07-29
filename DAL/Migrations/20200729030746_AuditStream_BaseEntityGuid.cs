using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AuditStream_BaseEntityGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "ApplyAuditStreamSolutionRules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "ApplyAuditStreamSolutionRules",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "ApplyAuditStreams",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "ApplyAuditStreams",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "ApplyAuditStreamNodeActions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "ApplyAuditStreamNodeActions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "ApplyAuditStreams");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "ApplyAuditStreams");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "ApplyAuditStreamNodeActions");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "ApplyAuditStreamNodeActions");
        }
    }
}
