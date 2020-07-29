using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class AuditStream_CompanyRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegionOnCompany",
                table: "ApplyAuditStreamSolutionRules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionOnCompany",
                table: "ApplyAuditStreams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionOnCompany",
                table: "ApplyAuditStreamNodeActions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegionOnCompany",
                table: "ApplyAuditStreamSolutionRules");

            migrationBuilder.DropColumn(
                name: "RegionOnCompany",
                table: "ApplyAuditStreams");

            migrationBuilder.DropColumn(
                name: "RegionOnCompany",
                table: "ApplyAuditStreamNodeActions");
        }
    }
}
