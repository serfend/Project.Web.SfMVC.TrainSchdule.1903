using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class applyAuditStreamStepInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstMemberCompanyName",
                table: "ApplyAuditSteps",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ApplyAuditSteps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstMemberCompanyName",
                table: "ApplyAuditSteps");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ApplyAuditSteps");
        }
    }
}
