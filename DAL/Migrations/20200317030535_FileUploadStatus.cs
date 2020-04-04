using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
	public partial class FileUploadStatus : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "UploadCaches",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UploadCaches", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "FileUploadStatuses",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					FileInfoId = table.Column<Guid>(nullable: true),
					Current = table.Column<long>(nullable: false),
					Total = table.Column<long>(nullable: false),
					UploadCacheId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FileUploadStatuses", x => x.Id);
					table.ForeignKey(
						name: "FK_FileUploadStatuses_UserFileInfos_FileInfoId",
						column: x => x.FileInfoId,
						principalTable: "UserFileInfos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_FileUploadStatuses_UploadCaches_UploadCacheId",
						column: x => x.UploadCacheId,
						principalTable: "UploadCaches",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_FileUploadStatuses_FileInfoId",
				table: "FileUploadStatuses",
				column: "FileInfoId");

			migrationBuilder.CreateIndex(
				name: "IX_FileUploadStatuses_UploadCacheId",
				table: "FileUploadStatuses",
				column: "UploadCacheId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "FileUploadStatuses");

			migrationBuilder.DropTable(
				name: "UploadCaches");
		}
	}
}