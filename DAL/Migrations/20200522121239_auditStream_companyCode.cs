using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class auditStream_companyCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstMemberCompanyCode",
                table: "ApplyAuditSteps",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstMemberCompanyCode",
                table: "ApplyAuditSteps");
        }
    }
}
