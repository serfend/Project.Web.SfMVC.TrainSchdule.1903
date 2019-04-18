using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainSchdule.DAL.Migrations
{
    public partial class AddUserInviter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvitedBy",
                table: "AppUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitedBy",
                table: "AppUsers");
        }
    }
}
