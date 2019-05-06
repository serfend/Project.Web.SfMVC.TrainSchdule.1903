using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Apply_SubmitBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_AppUsers_FromId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyRequests_RequestId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyStamps_stampId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_UserBaseInfo_BaseInfoId",
                table: "AppUsers");

            migrationBuilder.DropTable(
                name: "ApplyStamps");

            migrationBuilder.DropIndex(
                name: "IX_Applies_FromId",
                table: "Applies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBaseInfo",
                table: "UserBaseInfo");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "xjlb",
                table: "Applies");

            migrationBuilder.RenameTable(
                name: "UserBaseInfo",
                newName: "AppUserBaseInfos");

            migrationBuilder.RenameColumn(
                name: "xjts",
                table: "ApplyRequests",
                newName: "VocationLength");

            migrationBuilder.RenameColumn(
                name: "ltts",
                table: "ApplyRequests",
                newName: "OnTripLength");

            migrationBuilder.RenameColumn(
                name: "stampId",
                table: "Applies",
                newName: "RequestInfoId");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "Applies",
                newName: "BaseInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Applies_stampId",
                table: "Applies",
                newName: "IX_Applies_RequestInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Applies_RequestId",
                table: "Applies",
                newName: "IX_Applies_BaseInfoId");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "ApplyRequests",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StampLeave",
                table: "ApplyRequests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StampReturn",
                table: "ApplyRequests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VocationPlaceCode",
                table: "ApplyRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VocationType",
                table: "ApplyRequests",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserBaseInfos",
                table: "AppUserBaseInfos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApplyBaseInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromId = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    DutiesCode = table.Column<int>(nullable: true),
                    SocialId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyBaseInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyBaseInfos_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyBaseInfos_Duties_DutiesCode",
                        column: x => x.DutiesCode,
                        principalTable: "Duties",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyBaseInfos_AppUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyBaseInfos_UserSocialInfo_SocialId",
                        column: x => x.SocialId,
                        principalTable: "UserSocialInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyRequests_VocationPlaceCode",
                table: "ApplyRequests",
                column: "VocationPlaceCode");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_CompanyCode",
                table: "ApplyBaseInfos",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_DutiesCode",
                table: "ApplyBaseInfos",
                column: "DutiesCode");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_FromId",
                table: "ApplyBaseInfos",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_SocialId",
                table: "ApplyBaseInfos",
                column: "SocialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyBaseInfos_BaseInfoId",
                table: "Applies",
                column: "BaseInfoId",
                principalTable: "ApplyBaseInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyRequests_RequestInfoId",
                table: "Applies",
                column: "RequestInfoId",
                principalTable: "ApplyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VocationPlaceCode",
                table: "ApplyRequests",
                column: "VocationPlaceCode",
                principalTable: "AdminDivisions",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AppUserBaseInfos_BaseInfoId",
                table: "AppUsers",
                column: "BaseInfoId",
                principalTable: "AppUserBaseInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyBaseInfos_BaseInfoId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyRequests_RequestInfoId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyRequests_AdminDivisions_VocationPlaceCode",
                table: "ApplyRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AppUserBaseInfos_BaseInfoId",
                table: "AppUsers");

            migrationBuilder.DropTable(
                name: "ApplyBaseInfos");

            migrationBuilder.DropIndex(
                name: "IX_ApplyRequests_VocationPlaceCode",
                table: "ApplyRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserBaseInfos",
                table: "AppUserBaseInfos");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ApplyRequests");

            migrationBuilder.DropColumn(
                name: "StampLeave",
                table: "ApplyRequests");

            migrationBuilder.DropColumn(
                name: "StampReturn",
                table: "ApplyRequests");

            migrationBuilder.DropColumn(
                name: "VocationPlaceCode",
                table: "ApplyRequests");

            migrationBuilder.DropColumn(
                name: "VocationType",
                table: "ApplyRequests");

            migrationBuilder.RenameTable(
                name: "AppUserBaseInfos",
                newName: "UserBaseInfo");

            migrationBuilder.RenameColumn(
                name: "VocationLength",
                table: "ApplyRequests",
                newName: "xjts");

            migrationBuilder.RenameColumn(
                name: "OnTripLength",
                table: "ApplyRequests",
                newName: "ltts");

            migrationBuilder.RenameColumn(
                name: "RequestInfoId",
                table: "Applies",
                newName: "stampId");

            migrationBuilder.RenameColumn(
                name: "BaseInfoId",
                table: "Applies",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Applies_RequestInfoId",
                table: "Applies",
                newName: "IX_Applies_stampId");

            migrationBuilder.RenameIndex(
                name: "IX_Applies_BaseInfoId",
                table: "Applies",
                newName: "IX_Applies_RequestId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FromId",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "xjlb",
                table: "Applies",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBaseInfo",
                table: "UserBaseInfo",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ApplyStamps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    gdsj = table.Column<DateTime>(nullable: false),
                    ldsj = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyStamps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applies_FromId",
                table: "Applies",
                column: "FromId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_AppUsers_FromId",
                table: "Applies",
                column: "FromId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyRequests_RequestId",
                table: "Applies",
                column: "RequestId",
                principalTable: "ApplyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyStamps_stampId",
                table: "Applies",
                column: "stampId",
                principalTable: "ApplyStamps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_UserBaseInfo_BaseInfoId",
                table: "AppUsers",
                column: "BaseInfoId",
                principalTable: "UserBaseInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
