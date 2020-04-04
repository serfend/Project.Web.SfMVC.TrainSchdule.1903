using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
	public partial class GameR3 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsDescriptions_VocationStatisticsDescriptionId",
				table: "VocationStatisticsDescriptions");

			migrationBuilder.DropIndex(
				name: "IX_VocationStatisticsDescriptions_VocationStatisticsDescriptionId",
				table: "VocationStatisticsDescriptions");

			migrationBuilder.DropColumn(
				name: "VocationStatisticsDescriptionId",
				table: "VocationStatisticsDescriptions");

			migrationBuilder.CreateTable(
				name: "GameR3Users",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					GameId = table.Column<string>(nullable: true),
					Enable = table.Column<bool>(nullable: false),
					HandleInterval = table.Column<long>(nullable: false),
					LastHandleStamp = table.Column<long>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_GameR3Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "GameR3UserInfos",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					UserId = table.Column<Guid>(nullable: true),
					DateTime = table.Column<DateTime>(nullable: false),
					NickName = table.Column<string>(nullable: true),
					Level = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_GameR3UserInfos", x => x.Id);
					table.ForeignKey(
						name: "FK_GameR3UserInfos_GameR3Users_UserId",
						column: x => x.UserId,
						principalTable: "GameR3Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "GiftCodes",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Code = table.Column<string>(nullable: true),
					Valid = table.Column<bool>(nullable: false),
					StatusDescription = table.Column<string>(nullable: true),
					ShareTime = table.Column<DateTime>(nullable: false),
					InvalidTime = table.Column<DateTime>(nullable: false),
					ShareById = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_GiftCodes", x => x.Id);
					table.ForeignKey(
						name: "FK_GiftCodes_GameR3Users_ShareById",
						column: x => x.ShareById,
						principalTable: "GameR3Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "GainGiftCodeHistory",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					UserId = table.Column<Guid>(nullable: true),
					GainStamp = table.Column<long>(nullable: false),
					CodeId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_GainGiftCodeHistory", x => x.Id);
					table.ForeignKey(
						name: "FK_GainGiftCodeHistory_GiftCodes_CodeId",
						column: x => x.CodeId,
						principalTable: "GiftCodes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_GainGiftCodeHistory_GameR3Users_UserId",
						column: x => x.UserId,
						principalTable: "GameR3Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_GainGiftCodeHistory_CodeId",
				table: "GainGiftCodeHistory",
				column: "CodeId");

			migrationBuilder.CreateIndex(
				name: "IX_GainGiftCodeHistory_UserId",
				table: "GainGiftCodeHistory",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_GameR3UserInfos_UserId",
				table: "GameR3UserInfos",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_GiftCodes_ShareById",
				table: "GiftCodes",
				column: "ShareById");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "GainGiftCodeHistory");

			migrationBuilder.DropTable(
				name: "GameR3UserInfos");

			migrationBuilder.DropTable(
				name: "GiftCodes");

			migrationBuilder.DropTable(
				name: "GameR3Users");

			migrationBuilder.AddColumn<Guid>(
				name: "VocationStatisticsDescriptionId",
				table: "VocationStatisticsDescriptions",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_VocationStatisticsDescriptions_VocationStatisticsDescriptionId",
				table: "VocationStatisticsDescriptions",
				column: "VocationStatisticsDescriptionId");

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsDescriptions_VocationStatisticsDescriptionId",
				table: "VocationStatisticsDescriptions",
				column: "VocationStatisticsDescriptionId",
				principalTable: "VocationStatisticsDescriptions",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}