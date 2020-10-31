using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class TitleEnableVacationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserResumes");

            migrationBuilder.AddColumn<bool>(
                name: "EnableVacationDay",
                table: "UserCompanyTitles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AppUserTitleResumes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    ModelCode = table.Column<int>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    UserResumeInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTitleResumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserTitleResumes_UserCompanyTitles_ModelCode",
                        column: x => x.ModelCode,
                        principalTable: "UserCompanyTitles",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUserTitleResumes_AppUserResumeInfos_UserResumeInfoId",
                        column: x => x.UserResumeInfoId,
                        principalTable: "AppUserResumeInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSocialResumes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    ModelId = table.Column<Guid>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false),
                    SocialResumeType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSocialResumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSocialResumes_AppUserSocialInfoSettleMoments_ModelId",
                        column: x => x.ModelId,
                        principalTable: "AppUserSocialInfoSettleMoments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTitleResumes_ModelCode",
                table: "AppUserTitleResumes",
                column: "ModelCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTitleResumes_UserResumeInfoId",
                table: "AppUserTitleResumes",
                column: "UserResumeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialResumes_ModelId",
                table: "UserSocialResumes",
                column: "ModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserTitleResumes");

            migrationBuilder.DropTable(
                name: "UserSocialResumes");

            migrationBuilder.DropColumn(
                name: "EnableVacationDay",
                table: "UserCompanyTitles");

            migrationBuilder.CreateTable(
                name: "AppUserResumes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserResumes", x => x.Id);
                });
        }
    }
}
