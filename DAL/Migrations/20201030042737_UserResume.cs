using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class UserResume : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
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

			migrationBuilder.AddColumn<Guid>(
				name: "ResumeInfoId",
				table: "AppUsers",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "AppUserResumeInfos",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					IsRemoved = table.Column<bool>(nullable: false),
					IsRemovedDate = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppUserResumeInfos", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AppUserResumes",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					IsRemoved = table.Column<bool>(nullable: false),
					IsRemovedDate = table.Column<DateTime>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppUserResumes", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AppUsers_ResumeInfoId",
				table: "AppUsers",
				column: "ResumeInfoId");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUsers_AppUserResumeInfos_ResumeInfoId",
				table: "AppUsers",
				column: "ResumeInfoId",
				principalTable: "AppUserResumeInfos",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUsers_AppUserResumeInfos_ResumeInfoId",
				table: "AppUsers");

			migrationBuilder.DropTable(
				name: "AppUserResumeInfos");

			migrationBuilder.DropTable(
				name: "AppUserResumes");

			migrationBuilder.DropIndex(
				name: "IX_AppUsers_ResumeInfoId",
				table: "AppUsers");

			migrationBuilder.DropColumn(
				name: "ResumeInfoId",
				table: "AppUsers");

			migrationBuilder.AddColumn<Guid>(
				name: "TrainInfoId",
				table: "AppUsers",
				type: "uniqueidentifier",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "TrainRank",
				columns: table => new
				{
					Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TrainRank", x => x.Code);
				});

			migrationBuilder.CreateTable(
				name: "TrainType",
				columns: table => new
				{
					Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TrainType", x => x.Code);
				});

			migrationBuilder.CreateTable(
				name: "UserTrainInfo",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsRemoved = table.Column<bool>(type: "bit", nullable: false),
					IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserTrainInfo", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Train",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsRemoved = table.Column<bool>(type: "bit", nullable: false),
					IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					Time_Begin = table.Column<DateTime>(type: "datetime2", nullable: false),
					Time_End = table.Column<DateTime>(type: "datetime2", nullable: false),
					TrainName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					TrainRankCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
					TrainTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
					UserTrainInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
	}
}