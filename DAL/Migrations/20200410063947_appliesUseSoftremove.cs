using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class appliesUseSoftremove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserActions_Ip",
                table: "UserActions");

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "XlsTempletes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "XlsTempletes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "VocationStatisticsDescriptions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "VocationStatisticsDescriptions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "VocationStatisticsDescriptionDataStatusCounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "VocationStatisticsDescriptionDataStatusCounts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "VocationStatisticsDatas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "VocationStatisticsDatas",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "VocationAdditionals",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "VocationAdditionals",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "UserTrainInfo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "UserTrainInfo",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "UserFiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "UserFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "UserFileInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "UserFileInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "UserActions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "UserActions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "UserActions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "UploadCaches",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "UploadCaches",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Train",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "Train",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Subjects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "Subjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Standards",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "Standards",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "SignIns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "SignIns",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "RecallOrders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "RecallOrders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "PostContents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "PostContents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Permissions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "Permissions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "GiftCodes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "GiftCodes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "GameR3Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "GameR3Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "GameR3UserInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "GameR3UserInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "GainGiftCodeHistory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "GainGiftCodeHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "FileUploadStatuses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "FileUploadStatuses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "CompanyManagers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "CompanyManagers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AUserSocialInfoSettles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AUserSocialInfoSettles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AppUserSocialInfoSettleMoments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AppUserSocialInfoSettleMoments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AppUserSocialInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AppUserSocialInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AppUserDiyInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AppUserDiyInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AppUserDiyAvatars",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AppUserDiyAvatars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AppUserCompanyInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AppUserCompanyInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AppUserBaseInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AppUserBaseInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AppUserApplicationSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AppUserApplicationSettings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AppUserApplicationInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "AppUserApplicationInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "ApplyResponses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "ApplyResponses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "ApplyRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "ApplyRequests",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "ApplyBaseInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "ApplyBaseInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "ApplyAuditSteps",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "ApplyAuditSteps",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "Applies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsRemovedDate",
                table: "Applies",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "XlsTempletes");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "XlsTempletes");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "VocationStatisticsDescriptions");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "VocationStatisticsDescriptions");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "VocationStatisticsDescriptionDataStatusCounts");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "VocationStatisticsDescriptionDataStatusCounts");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "VocationStatisticsDatas");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "VocationStatisticsDatas");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "VocationAdditionals");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "VocationAdditionals");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "UserTrainInfo");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "UserTrainInfo");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "UserFiles");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "UserFiles");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "UserFileInfos");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "UserFileInfos");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "UserActions");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "UploadCaches");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "UploadCaches");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Train");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "Train");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Standards");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "Standards");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "SignIns");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "SignIns");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "RecallOrders");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "RecallOrders");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "PostContents");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "PostContents");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "GiftCodes");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "GiftCodes");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "GameR3Users");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "GameR3Users");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "GameR3UserInfos");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "GameR3UserInfos");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "GainGiftCodeHistory");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "GainGiftCodeHistory");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "FileUploadStatuses");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "FileUploadStatuses");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "CompanyManagers");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "CompanyManagers");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AUserSocialInfoSettles");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AUserSocialInfoSettles");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AppUserSocialInfoSettleMoments");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AppUserSocialInfoSettleMoments");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AppUserSocialInfos");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AppUserSocialInfos");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AppUserDiyInfos");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AppUserDiyInfos");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AppUserDiyAvatars");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AppUserDiyAvatars");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AppUserCompanyInfos");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AppUserCompanyInfos");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AppUserBaseInfos");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AppUserBaseInfos");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AppUserApplicationSettings");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AppUserApplicationSettings");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "AppUserApplicationInfos");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "ApplyResponses");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "ApplyResponses");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "ApplyRequests");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "ApplyRequests");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "ApplyBaseInfos");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "ApplyBaseInfos");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "ApplyAuditSteps");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "ApplyAuditSteps");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "Applies");

            migrationBuilder.DropColumn(
                name: "IsRemovedDate",
                table: "Applies");

            migrationBuilder.AlterColumn<string>(
                name: "Ip",
                table: "UserActions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserActions_Ip",
                table: "UserActions",
                column: "Ip");
        }
    }
}
