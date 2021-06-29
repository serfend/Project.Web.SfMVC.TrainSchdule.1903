using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class UserCompanyOfManage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyOfManageCode",
                table: "AppUserCompanyInfos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserCompanyInfos_CompanyOfManageCode",
                table: "AppUserCompanyInfos",
                column: "CompanyOfManageCode");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserCompanyInfos_Companies_CompanyOfManageCode",
                table: "AppUserCompanyInfos",
                column: "CompanyOfManageCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserCompanyInfos_Companies_CompanyOfManageCode",
                table: "AppUserCompanyInfos");

            migrationBuilder.DropIndex(
                name: "IX_AppUserCompanyInfos_CompanyOfManageCode",
                table: "AppUserCompanyInfos");

            migrationBuilder.DropColumn(
                name: "CompanyOfManageCode",
                table: "AppUserCompanyInfos");
        }
    }
}
