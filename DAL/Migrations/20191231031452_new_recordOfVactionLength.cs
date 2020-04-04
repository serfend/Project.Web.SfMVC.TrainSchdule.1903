using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
	public partial class new_recordOfVactionLength : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "VacationDay",
				table: "UserCompanyTitles",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<DateTime>(
				name: "TitleDate",
				table: "AppUserCompanyInfos",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "VacationModefyRecord",
				columns: table => new
				{
					Code = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Length = table.Column<double>(nullable: false),
					UpdateDate = table.Column<DateTime>(nullable: false),
					SettleId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_VacationModefyRecord", x => x.Code);
					table.ForeignKey(
						name: "FK_VacationModefyRecord_AUserSocialInfoSettles_SettleId",
						column: x => x.SettleId,
						principalTable: "AUserSocialInfoSettles",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_VacationModefyRecord_SettleId",
				table: "VacationModefyRecord",
				column: "SettleId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "VacationModefyRecord");

			migrationBuilder.DropColumn(
				name: "VacationDay",
				table: "UserCompanyTitles");

			migrationBuilder.DropColumn(
				name: "TitleDate",
				table: "AppUserCompanyInfos");
		}
	}
}