using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class settle_vacationLength_record : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrevYearlyLength",
                table: "AUserSocialInfoSettles");

            migrationBuilder.DropColumn(
                name: "LastVocationUpdateTime",
                table: "AppUserApplicationSettings");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "VacationModefyRecord",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNewYearInitData",
                table: "VacationModefyRecord",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "VacationModefyRecord");

            migrationBuilder.DropColumn(
                name: "IsNewYearInitData",
                table: "VacationModefyRecord");

            migrationBuilder.AddColumn<int>(
                name: "PrevYearlyLength",
                table: "AUserSocialInfoSettles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVocationUpdateTime",
                table: "AppUserApplicationSettings",
                nullable: true);
        }
    }
}
