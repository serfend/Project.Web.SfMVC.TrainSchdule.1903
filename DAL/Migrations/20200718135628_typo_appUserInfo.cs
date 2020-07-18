using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class typo_appUserInfo : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfos_AUserSocialInfoSettles_SettleId",
				table: "AppUserSocialInfos");

			migrationBuilder.DropForeignKey(
				name: "FK_AppUsersSettleModefyRecord_AUserSocialInfoSettles_SettleId",
				table: "AppUsersSettleModefyRecord");

			migrationBuilder.DropForeignKey(
				name: "FK_AUserSocialInfoSettles_AppUserSocialInfoSettleMoments_LoverId",
				table: "AUserSocialInfoSettles");

			migrationBuilder.DropForeignKey(
				name: "FK_AUserSocialInfoSettles_AppUserSocialInfoSettleMoments_LoversParentId",
				table: "AUserSocialInfoSettles");

			migrationBuilder.DropForeignKey(
				name: "FK_AUserSocialInfoSettles_AppUserSocialInfoSettleMoments_ParentId",
				table: "AUserSocialInfoSettles");

			migrationBuilder.DropForeignKey(
				name: "FK_AUserSocialInfoSettles_AppUserSocialInfoSettleMoments_SelfId",
				table: "AUserSocialInfoSettles");

			migrationBuilder.DropPrimaryKey(
				name: "PK_AUserSocialInfoSettles",
				table: "AUserSocialInfoSettles");

			migrationBuilder.RenameTable(
				name: "AUserSocialInfoSettles",
				newName: "AppUserSocialInfoSettles");

			migrationBuilder.RenameIndex(
				name: "IX_AUserSocialInfoSettles_SelfId",
				table: "AppUserSocialInfoSettles",
				newName: "IX_AppUserSocialInfoSettles_SelfId");

			migrationBuilder.RenameIndex(
				name: "IX_AUserSocialInfoSettles_ParentId",
				table: "AppUserSocialInfoSettles",
				newName: "IX_AppUserSocialInfoSettles_ParentId");

			migrationBuilder.RenameIndex(
				name: "IX_AUserSocialInfoSettles_LoversParentId",
				table: "AppUserSocialInfoSettles",
				newName: "IX_AppUserSocialInfoSettles_LoversParentId");

			migrationBuilder.RenameIndex(
				name: "IX_AUserSocialInfoSettles_LoverId",
				table: "AppUserSocialInfoSettles",
				newName: "IX_AppUserSocialInfoSettles_LoverId");

			migrationBuilder.AddPrimaryKey(
				name: "PK_AppUserSocialInfoSettles",
				table: "AppUserSocialInfoSettles",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfos_AppUserSocialInfoSettles_SettleId",
				table: "AppUserSocialInfos",
				column: "SettleId",
				principalTable: "AppUserSocialInfoSettles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfoSettles_AppUserSocialInfoSettleMoments_LoverId",
				table: "AppUserSocialInfoSettles",
				column: "LoverId",
				principalTable: "AppUserSocialInfoSettleMoments",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfoSettles_AppUserSocialInfoSettleMoments_LoversParentId",
				table: "AppUserSocialInfoSettles",
				column: "LoversParentId",
				principalTable: "AppUserSocialInfoSettleMoments",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfoSettles_AppUserSocialInfoSettleMoments_ParentId",
				table: "AppUserSocialInfoSettles",
				column: "ParentId",
				principalTable: "AppUserSocialInfoSettleMoments",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfoSettles_AppUserSocialInfoSettleMoments_SelfId",
				table: "AppUserSocialInfoSettles",
				column: "SelfId",
				principalTable: "AppUserSocialInfoSettleMoments",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AppUsersSettleModefyRecord_AppUserSocialInfoSettles_SettleId",
				table: "AppUsersSettleModefyRecord",
				column: "SettleId",
				principalTable: "AppUserSocialInfoSettles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfos_AppUserSocialInfoSettles_SettleId",
				table: "AppUserSocialInfos");

			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfoSettles_AppUserSocialInfoSettleMoments_LoverId",
				table: "AppUserSocialInfoSettles");

			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfoSettles_AppUserSocialInfoSettleMoments_LoversParentId",
				table: "AppUserSocialInfoSettles");

			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfoSettles_AppUserSocialInfoSettleMoments_ParentId",
				table: "AppUserSocialInfoSettles");

			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfoSettles_AppUserSocialInfoSettleMoments_SelfId",
				table: "AppUserSocialInfoSettles");

			migrationBuilder.DropForeignKey(
				name: "FK_AppUsersSettleModefyRecord_AppUserSocialInfoSettles_SettleId",
				table: "AppUsersSettleModefyRecord");

			migrationBuilder.DropPrimaryKey(
				name: "PK_AppUserSocialInfoSettles",
				table: "AppUserSocialInfoSettles");

			migrationBuilder.RenameTable(
				name: "AppUserSocialInfoSettles",
				newName: "AUserSocialInfoSettles");

			migrationBuilder.RenameIndex(
				name: "IX_AppUserSocialInfoSettles_SelfId",
				table: "AUserSocialInfoSettles",
				newName: "IX_AUserSocialInfoSettles_SelfId");

			migrationBuilder.RenameIndex(
				name: "IX_AppUserSocialInfoSettles_ParentId",
				table: "AUserSocialInfoSettles",
				newName: "IX_AUserSocialInfoSettles_ParentId");

			migrationBuilder.RenameIndex(
				name: "IX_AppUserSocialInfoSettles_LoversParentId",
				table: "AUserSocialInfoSettles",
				newName: "IX_AUserSocialInfoSettles_LoversParentId");

			migrationBuilder.RenameIndex(
				name: "IX_AppUserSocialInfoSettles_LoverId",
				table: "AUserSocialInfoSettles",
				newName: "IX_AUserSocialInfoSettles_LoverId");

			migrationBuilder.AddPrimaryKey(
				name: "PK_AUserSocialInfoSettles",
				table: "AUserSocialInfoSettles",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfos_AUserSocialInfoSettles_SettleId",
				table: "AppUserSocialInfos",
				column: "SettleId",
				principalTable: "AUserSocialInfoSettles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AppUsersSettleModefyRecord_AUserSocialInfoSettles_SettleId",
				table: "AppUsersSettleModefyRecord",
				column: "SettleId",
				principalTable: "AUserSocialInfoSettles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AUserSocialInfoSettles_AppUserSocialInfoSettleMoments_LoverId",
				table: "AUserSocialInfoSettles",
				column: "LoverId",
				principalTable: "AppUserSocialInfoSettleMoments",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AUserSocialInfoSettles_AppUserSocialInfoSettleMoments_LoversParentId",
				table: "AUserSocialInfoSettles",
				column: "LoversParentId",
				principalTable: "AppUserSocialInfoSettleMoments",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AUserSocialInfoSettles_AppUserSocialInfoSettleMoments_ParentId",
				table: "AUserSocialInfoSettles",
				column: "ParentId",
				principalTable: "AppUserSocialInfoSettleMoments",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AUserSocialInfoSettles_AppUserSocialInfoSettleMoments_SelfId",
				table: "AUserSocialInfoSettles",
				column: "SelfId",
				principalTable: "AppUserSocialInfoSettleMoments",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}