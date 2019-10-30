using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyBaseInfos_UserSocialInfo_SocialId",
                table: "ApplyBaseInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_UserApplicationInfo_ApplicationId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_UserCompanyInfo_CompanyInfoId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_UserSocialInfo_SocialInfoId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserApplicationInfo_Permissions_PermissionId",
                table: "UserApplicationInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCompanyInfo_Companies_CompanyCode",
                table: "UserCompanyInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCompanyInfo_Duties_DutiesCode",
                table: "UserCompanyInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSocialInfo_AdminDivisions_AddressCode",
                table: "UserSocialInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSocialInfo",
                table: "UserSocialInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCompanyInfo",
                table: "UserCompanyInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserApplicationInfo",
                table: "UserApplicationInfo");

            migrationBuilder.RenameTable(
                name: "UserSocialInfo",
                newName: "AppUserSocialInfos");

            migrationBuilder.RenameTable(
                name: "UserCompanyInfo",
                newName: "AppUserCompanyInfos");

            migrationBuilder.RenameTable(
                name: "UserApplicationInfo",
                newName: "AppUserApplicationInfos");

            migrationBuilder.RenameIndex(
                name: "IX_UserSocialInfo_AddressCode",
                table: "AppUserSocialInfos",
                newName: "IX_AppUserSocialInfos_AddressCode");

            migrationBuilder.RenameIndex(
                name: "IX_UserCompanyInfo_DutiesCode",
                table: "AppUserCompanyInfos",
                newName: "IX_AppUserCompanyInfos_DutiesCode");

            migrationBuilder.RenameIndex(
                name: "IX_UserCompanyInfo_CompanyCode",
                table: "AppUserCompanyInfos",
                newName: "IX_AppUserCompanyInfos_CompanyCode");

            migrationBuilder.RenameIndex(
                name: "IX_UserApplicationInfo_PermissionId",
                table: "AppUserApplicationInfos",
                newName: "IX_AppUserApplicationInfos_PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserSocialInfos",
                table: "AppUserSocialInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserCompanyInfos",
                table: "AppUserCompanyInfos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserApplicationInfos",
                table: "AppUserApplicationInfos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CompanyManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    AuthById = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyManagers_AppUsers_AuthById",
                        column: x => x.AuthById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyManagers_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyManagers_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyManagers_AuthById",
                table: "CompanyManagers",
                column: "AuthById");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyManagers_CompanyCode",
                table: "CompanyManagers",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyManagers_UserId",
                table: "CompanyManagers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyBaseInfos_AppUserSocialInfos_SocialId",
                table: "ApplyBaseInfos",
                column: "SocialId",
                principalTable: "AppUserSocialInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserApplicationInfos_Permissions_PermissionId",
                table: "AppUserApplicationInfos",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserCompanyInfos_Companies_CompanyCode",
                table: "AppUserCompanyInfos",
                column: "CompanyCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserCompanyInfos_Duties_DutiesCode",
                table: "AppUserCompanyInfos",
                column: "DutiesCode",
                principalTable: "Duties",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AppUserApplicationInfos_ApplicationId",
                table: "AppUsers",
                column: "ApplicationId",
                principalTable: "AppUserApplicationInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AppUserCompanyInfos_CompanyInfoId",
                table: "AppUsers",
                column: "CompanyInfoId",
                principalTable: "AppUserCompanyInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AppUserSocialInfos_SocialInfoId",
                table: "AppUsers",
                column: "SocialInfoId",
                principalTable: "AppUserSocialInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserSocialInfos_AdminDivisions_AddressCode",
                table: "AppUserSocialInfos",
                column: "AddressCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyBaseInfos_AppUserSocialInfos_SocialId",
                table: "ApplyBaseInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserApplicationInfos_Permissions_PermissionId",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserCompanyInfos_Companies_CompanyCode",
                table: "AppUserCompanyInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserCompanyInfos_Duties_DutiesCode",
                table: "AppUserCompanyInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AppUserApplicationInfos_ApplicationId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AppUserCompanyInfos_CompanyInfoId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AppUserSocialInfos_SocialInfoId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserSocialInfos_AdminDivisions_AddressCode",
                table: "AppUserSocialInfos");

            migrationBuilder.DropTable(
                name: "CompanyManagers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserSocialInfos",
                table: "AppUserSocialInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserCompanyInfos",
                table: "AppUserCompanyInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserApplicationInfos",
                table: "AppUserApplicationInfos");

            migrationBuilder.RenameTable(
                name: "AppUserSocialInfos",
                newName: "UserSocialInfo");

            migrationBuilder.RenameTable(
                name: "AppUserCompanyInfos",
                newName: "UserCompanyInfo");

            migrationBuilder.RenameTable(
                name: "AppUserApplicationInfos",
                newName: "UserApplicationInfo");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserSocialInfos_AddressCode",
                table: "UserSocialInfo",
                newName: "IX_UserSocialInfo_AddressCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserCompanyInfos_DutiesCode",
                table: "UserCompanyInfo",
                newName: "IX_UserCompanyInfo_DutiesCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserCompanyInfos_CompanyCode",
                table: "UserCompanyInfo",
                newName: "IX_UserCompanyInfo_CompanyCode");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserApplicationInfos_PermissionId",
                table: "UserApplicationInfo",
                newName: "IX_UserApplicationInfo_PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSocialInfo",
                table: "UserSocialInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCompanyInfo",
                table: "UserCompanyInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserApplicationInfo",
                table: "UserApplicationInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyBaseInfos_UserSocialInfo_SocialId",
                table: "ApplyBaseInfos",
                column: "SocialId",
                principalTable: "UserSocialInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_UserApplicationInfo_ApplicationId",
                table: "AppUsers",
                column: "ApplicationId",
                principalTable: "UserApplicationInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_UserCompanyInfo_CompanyInfoId",
                table: "AppUsers",
                column: "CompanyInfoId",
                principalTable: "UserCompanyInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_UserSocialInfo_SocialInfoId",
                table: "AppUsers",
                column: "SocialInfoId",
                principalTable: "UserSocialInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserApplicationInfo_Permissions_PermissionId",
                table: "UserApplicationInfo",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCompanyInfo_Companies_CompanyCode",
                table: "UserCompanyInfo",
                column: "CompanyCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCompanyInfo_Duties_DutiesCode",
                table: "UserCompanyInfo",
                column: "DutiesCode",
                principalTable: "Duties",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSocialInfo_AdminDivisions_AddressCode",
                table: "UserSocialInfo",
                column: "AddressCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
