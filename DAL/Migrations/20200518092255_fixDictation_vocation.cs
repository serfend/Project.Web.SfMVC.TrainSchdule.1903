using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class fixDictation_vocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VocationPlaceCode",
                table: "ApplyRequests");

            migrationBuilder.RenameColumn(
                name: "MembersVocationDayLessThanP60",
                table: "VocationStatisticsDatas",
                newName: "MembersVacationDayLessThanP60");

            migrationBuilder.RenameColumn(
                name: "CompleteYearlyVocationCount",
                table: "VocationStatisticsDatas",
                newName: "CompleteYearlyVacationCount");

            migrationBuilder.RenameColumn(
                name: "CompleteVocationRealDayCount",
                table: "VocationStatisticsDatas",
                newName: "CompleteVacationRealDayCount");

            migrationBuilder.RenameColumn(
                name: "CompleteVocationExpectDayCount",
                table: "VocationStatisticsDatas",
                newName: "CompleteVacationExpectDayCount");

            migrationBuilder.RenameColumn(
                name: "VocationPlaceCode",
                table: "ApplyRequests",
                newName: "VacationPlaceCode");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyRequests_VocationPlaceCode",
                table: "ApplyRequests",
                newName: "IX_ApplyRequests_VacationPlaceCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyRequests",
                column: "VacationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VacationPlaceCode",
                table: "ApplyRequests");

            migrationBuilder.RenameColumn(
                name: "MembersVacationDayLessThanP60",
                table: "VocationStatisticsDatas",
                newName: "MembersVocationDayLessThanP60");

            migrationBuilder.RenameColumn(
                name: "CompleteYearlyVacationCount",
                table: "VocationStatisticsDatas",
                newName: "CompleteYearlyVocationCount");

            migrationBuilder.RenameColumn(
                name: "CompleteVacationRealDayCount",
                table: "VocationStatisticsDatas",
                newName: "CompleteVocationRealDayCount");

            migrationBuilder.RenameColumn(
                name: "CompleteVacationExpectDayCount",
                table: "VocationStatisticsDatas",
                newName: "CompleteVocationExpectDayCount");

            migrationBuilder.RenameColumn(
                name: "VacationPlaceCode",
                table: "ApplyRequests",
                newName: "VocationPlaceCode");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyRequests_VacationPlaceCode",
                table: "ApplyRequests",
                newName: "IX_ApplyRequests_VocationPlaceCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VocationPlaceCode",
                table: "ApplyRequests",
                column: "VocationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
