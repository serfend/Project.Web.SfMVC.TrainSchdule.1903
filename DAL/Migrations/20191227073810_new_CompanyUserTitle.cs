using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
	public partial class new_CompanyUserTitle : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "TitleCode",
				table: "AppUserCompanyInfos",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "PostContents",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					ReplySubjectId = table.Column<Guid>(nullable: true),
					Title = table.Column<string>(nullable: true),
					Contents = table.Column<string>(nullable: true),
					Create = table.Column<DateTime>(nullable: false),
					CreateById = table.Column<string>(nullable: true),
					ReplyToId = table.Column<string>(nullable: true),
					Discriminator = table.Column<string>(nullable: false),
					PostId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PostContents", x => x.Id);
					table.ForeignKey(
						name: "FK_PostContents_AppUsers_CreateById",
						column: x => x.CreateById,
						principalTable: "AppUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_PostContents_PostContents_PostId",
						column: x => x.PostId,
						principalTable: "PostContents",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_PostContents_PostContents_ReplySubjectId",
						column: x => x.ReplySubjectId,
						principalTable: "PostContents",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_PostContents_AppUsers_ReplyToId",
						column: x => x.ReplyToId,
						principalTable: "AppUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "UserCompanyTitles",
				columns: table => new
				{
					Code = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(nullable: true),
					Level = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserCompanyTitles", x => x.Code);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AppUserCompanyInfos_TitleCode",
				table: "AppUserCompanyInfos",
				column: "TitleCode");

			migrationBuilder.CreateIndex(
				name: "IX_PostContents_CreateById",
				table: "PostContents",
				column: "CreateById");

			migrationBuilder.CreateIndex(
				name: "IX_PostContents_PostId",
				table: "PostContents",
				column: "PostId");

			migrationBuilder.CreateIndex(
				name: "IX_PostContents_ReplySubjectId",
				table: "PostContents",
				column: "ReplySubjectId");

			migrationBuilder.CreateIndex(
				name: "IX_PostContents_ReplyToId",
				table: "PostContents",
				column: "ReplyToId");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserCompanyInfos_UserCompanyTitles_TitleCode",
				table: "AppUserCompanyInfos",
				column: "TitleCode",
				principalTable: "UserCompanyTitles",
				principalColumn: "Code",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUserCompanyInfos_UserCompanyTitles_TitleCode",
				table: "AppUserCompanyInfos");

			migrationBuilder.DropTable(
				name: "PostContents");

			migrationBuilder.DropTable(
				name: "UserCompanyTitles");

			migrationBuilder.DropIndex(
				name: "IX_AppUserCompanyInfos_TitleCode",
				table: "AppUserCompanyInfos");

			migrationBuilder.DropColumn(
				name: "TitleCode",
				table: "AppUserCompanyInfos");
		}
	}
}