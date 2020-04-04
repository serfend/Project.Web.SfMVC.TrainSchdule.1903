using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class vocationStatistics3 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplyCountId",
				table: "VocationStatisticsData");

			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplyMembersCountId",
				table: "VocationStatisticsData");

			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplySumDayCountId",
				table: "VocationStatisticsData");

			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsData_CurrentLevelStatisticsId",
				table: "VocationStatisticsDescriptions");

			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsData_IncludeChildLevelStatisticsId",
				table: "VocationStatisticsDescriptions");

			migrationBuilder.DropPrimaryKey(
				name: "PK_VocationStatisticsDescriptionDataStatusCount",
				table: "VocationStatisticsDescriptionDataStatusCount");

			migrationBuilder.DropPrimaryKey(
				name: "PK_VocationStatisticsData",
				table: "VocationStatisticsData");

			migrationBuilder.RenameTable(
				name: "VocationStatisticsDescriptionDataStatusCount",
				newName: "VocationStatisticsDescriptionDataStatusCounts");

			migrationBuilder.RenameTable(
				name: "VocationStatisticsData",
				newName: "VocationStatisticsDatas");

			migrationBuilder.RenameIndex(
				name: "IX_VocationStatisticsData_ApplySumDayCountId",
				table: "VocationStatisticsDatas",
				newName: "IX_VocationStatisticsDatas_ApplySumDayCountId");

			migrationBuilder.RenameIndex(
				name: "IX_VocationStatisticsData_ApplyMembersCountId",
				table: "VocationStatisticsDatas",
				newName: "IX_VocationStatisticsDatas_ApplyMembersCountId");

			migrationBuilder.RenameIndex(
				name: "IX_VocationStatisticsData_ApplyCountId",
				table: "VocationStatisticsDatas",
				newName: "IX_VocationStatisticsDatas_ApplyCountId");

			migrationBuilder.AddPrimaryKey(
				name: "PK_VocationStatisticsDescriptionDataStatusCounts",
				table: "VocationStatisticsDescriptionDataStatusCounts",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_VocationStatisticsDatas",
				table: "VocationStatisticsDatas",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplyCountId",
				table: "VocationStatisticsDatas",
				column: "ApplyCountId",
				principalTable: "VocationStatisticsDescriptionDataStatusCounts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplyMembersCountId",
				table: "VocationStatisticsDatas",
				column: "ApplyMembersCountId",
				principalTable: "VocationStatisticsDescriptionDataStatusCounts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplySumDayCountId",
				table: "VocationStatisticsDatas",
				column: "ApplySumDayCountId",
				principalTable: "VocationStatisticsDescriptionDataStatusCounts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsDatas_CurrentLevelStatisticsId",
				table: "VocationStatisticsDescriptions",
				column: "CurrentLevelStatisticsId",
				principalTable: "VocationStatisticsDatas",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsDatas_IncludeChildLevelStatisticsId",
				table: "VocationStatisticsDescriptions",
				column: "IncludeChildLevelStatisticsId",
				principalTable: "VocationStatisticsDatas",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplyCountId",
				table: "VocationStatisticsDatas");

			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplyMembersCountId",
				table: "VocationStatisticsDatas");

			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsDatas_VocationStatisticsDescriptionDataStatusCounts_ApplySumDayCountId",
				table: "VocationStatisticsDatas");

			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsDatas_CurrentLevelStatisticsId",
				table: "VocationStatisticsDescriptions");

			migrationBuilder.DropForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsDatas_IncludeChildLevelStatisticsId",
				table: "VocationStatisticsDescriptions");

			migrationBuilder.DropPrimaryKey(
				name: "PK_VocationStatisticsDescriptionDataStatusCounts",
				table: "VocationStatisticsDescriptionDataStatusCounts");

			migrationBuilder.DropPrimaryKey(
				name: "PK_VocationStatisticsDatas",
				table: "VocationStatisticsDatas");

			migrationBuilder.RenameTable(
				name: "VocationStatisticsDescriptionDataStatusCounts",
				newName: "VocationStatisticsDescriptionDataStatusCount");

			migrationBuilder.RenameTable(
				name: "VocationStatisticsDatas",
				newName: "VocationStatisticsData");

			migrationBuilder.RenameIndex(
				name: "IX_VocationStatisticsDatas_ApplySumDayCountId",
				table: "VocationStatisticsData",
				newName: "IX_VocationStatisticsData_ApplySumDayCountId");

			migrationBuilder.RenameIndex(
				name: "IX_VocationStatisticsDatas_ApplyMembersCountId",
				table: "VocationStatisticsData",
				newName: "IX_VocationStatisticsData_ApplyMembersCountId");

			migrationBuilder.RenameIndex(
				name: "IX_VocationStatisticsDatas_ApplyCountId",
				table: "VocationStatisticsData",
				newName: "IX_VocationStatisticsData_ApplyCountId");

			migrationBuilder.AddPrimaryKey(
				name: "PK_VocationStatisticsDescriptionDataStatusCount",
				table: "VocationStatisticsDescriptionDataStatusCount",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_VocationStatisticsData",
				table: "VocationStatisticsData",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplyCountId",
				table: "VocationStatisticsData",
				column: "ApplyCountId",
				principalTable: "VocationStatisticsDescriptionDataStatusCount",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplyMembersCountId",
				table: "VocationStatisticsData",
				column: "ApplyMembersCountId",
				principalTable: "VocationStatisticsDescriptionDataStatusCount",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsData_VocationStatisticsDescriptionDataStatusCount_ApplySumDayCountId",
				table: "VocationStatisticsData",
				column: "ApplySumDayCountId",
				principalTable: "VocationStatisticsDescriptionDataStatusCount",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsData_CurrentLevelStatisticsId",
				table: "VocationStatisticsDescriptions",
				column: "CurrentLevelStatisticsId",
				principalTable: "VocationStatisticsData",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_VocationStatisticsDescriptions_VocationStatisticsData_IncludeChildLevelStatisticsId",
				table: "VocationStatisticsDescriptions",
				column: "IncludeChildLevelStatisticsId",
				principalTable: "VocationStatisticsData",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}