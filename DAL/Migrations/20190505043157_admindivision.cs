using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class admindivision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "UserSocialInfo",
                newName: "AddressCode");

            migrationBuilder.AlterColumn<string>(
                name: "AddressCode",
                table: "UserSocialInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AdminDivisions",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminDivisions", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialInfo_AddressCode",
                table: "UserSocialInfo",
                column: "AddressCode");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialInfo_AdminDivisions_AddressCode",
                table: "UserSocialInfo",
                column: "AddressCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialInfo_AdminDivisions_AddressCode",
                table: "UserSocialInfo");

            migrationBuilder.DropTable(
                name: "AdminDivisions");

            migrationBuilder.DropIndex(
                name: "IX_UserSocialInfo_AddressCode",
                table: "UserSocialInfo");

            migrationBuilder.RenameColumn(
                name: "AddressCode",
                table: "UserSocialInfo",
                newName: "Address");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "UserSocialInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
