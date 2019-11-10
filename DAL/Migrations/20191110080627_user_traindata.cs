using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class user_traindata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainInfoId",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time_BirthDay",
                table: "AppUserBaseInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Time_Party",
                table: "AppUserBaseInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Time_Work",
                table: "AppUserBaseInfos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVocationUpdateTime",
                table: "AppUserApplicationSettings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TrainRank",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainRank", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "TrainType",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainType", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "UserTrainInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Train",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Time_Begin = table.Column<DateTime>(nullable: false),
                    Time_End = table.Column<DateTime>(nullable: false),
                    TrainName = table.Column<string>(nullable: true),
                    TrainRankCode = table.Column<string>(nullable: true),
                    TrainTypeCode = table.Column<string>(nullable: true),
                    UserTrainInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Train", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Train_TrainRank_TrainRankCode",
                        column: x => x.TrainRankCode,
                        principalTable: "TrainRank",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Train_TrainType_TrainTypeCode",
                        column: x => x.TrainTypeCode,
                        principalTable: "TrainType",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Train_UserTrainInfo_UserTrainInfoId",
                        column: x => x.UserTrainInfoId,
                        principalTable: "UserTrainInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_TrainInfoId",
                table: "AppUsers",
                column: "TrainInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Train_TrainRankCode",
                table: "Train",
                column: "TrainRankCode");

            migrationBuilder.CreateIndex(
                name: "IX_Train_TrainTypeCode",
                table: "Train",
                column: "TrainTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Train_UserTrainInfoId",
                table: "Train",
                column: "UserTrainInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_UserTrainInfo_TrainInfoId",
                table: "AppUsers",
                column: "TrainInfoId",
                principalTable: "UserTrainInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_UserTrainInfo_TrainInfoId",
                table: "AppUsers");

            migrationBuilder.DropTable(
                name: "Train");

            migrationBuilder.DropTable(
                name: "TrainRank");

            migrationBuilder.DropTable(
                name: "TrainType");

            migrationBuilder.DropTable(
                name: "UserTrainInfo");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_TrainInfoId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TrainInfoId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Time_BirthDay",
                table: "AppUserBaseInfos");

            migrationBuilder.DropColumn(
                name: "Time_Party",
                table: "AppUserBaseInfos");

            migrationBuilder.DropColumn(
                name: "Time_Work",
                table: "AppUserBaseInfos");

            migrationBuilder.DropColumn(
                name: "LastVocationUpdateTime",
                table: "AppUserApplicationSettings");
        }
    }
}
