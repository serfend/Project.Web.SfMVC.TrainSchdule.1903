using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class NewApplyType_InDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "ApplyAuditStreamSolutionRules",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "ApplyAuditStreamSolutionRules");
        }
    }
}
