using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class new_CompanyUserTitle3 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfos_Settles_SettleId",
				table: "AppUserSocialInfos");

			migrationBuilder.DropForeignKey(
				name: "FK_Moment_AdminDivisions_AddressCode",
				table: "Moment");

			migrationBuilder.DropForeignKey(
				name: "FK_Settles_Moment_LoverId",
				table: "Settles");

			migrationBuilder.DropForeignKey(
				name: "FK_Settles_Moment_LoversParentId",
				table: "Settles");

			migrationBuilder.DropForeignKey(
				name: "FK_Settles_Moment_ParentId",
				table: "Settles");

			migrationBuilder.DropForeignKey(
				name: "FK_Settles_Moment_SelfId",
				table: "Settles");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Settles",
				table: "Settles");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Moment",
				table: "Moment");

			migrationBuilder.RenameTable(
				name: "Settles",
				newName: "AUserSocialInfoSettles");

			migrationBuilder.RenameTable(
				name: "Moment",
				newName: "AppUserSocialInfoSettleMoments");

			migrationBuilder.RenameIndex(
				name: "IX_Settles_SelfId",
				table: "AUserSocialInfoSettles",
				newName: "IX_AUserSocialInfoSettles_SelfId");

			migrationBuilder.RenameIndex(
				name: "IX_Settles_ParentId",
				table: "AUserSocialInfoSettles",
				newName: "IX_AUserSocialInfoSettles_ParentId");

			migrationBuilder.RenameIndex(
				name: "IX_Settles_LoversParentId",
				table: "AUserSocialInfoSettles",
				newName: "IX_AUserSocialInfoSettles_LoversParentId");

			migrationBuilder.RenameIndex(
				name: "IX_Settles_LoverId",
				table: "AUserSocialInfoSettles",
				newName: "IX_AUserSocialInfoSettles_LoverId");

			migrationBuilder.RenameIndex(
				name: "IX_Moment_AddressCode",
				table: "AppUserSocialInfoSettleMoments",
				newName: "IX_AppUserSocialInfoSettleMoments_AddressCode");

			migrationBuilder.AddPrimaryKey(
				name: "PK_AUserSocialInfoSettles",
				table: "AUserSocialInfoSettles",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_AppUserSocialInfoSettleMoments",
				table: "AppUserSocialInfoSettleMoments",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfos_AUserSocialInfoSettles_SettleId",
				table: "AppUserSocialInfos",
				column: "SettleId",
				principalTable: "AUserSocialInfoSettles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfoSettleMoments_AdminDivisions_AddressCode",
				table: "AppUserSocialInfoSettleMoments",
				column: "AddressCode",
				principalTable: "AdminDivisions",
				principalColumn: "Code",
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

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfos_AUserSocialInfoSettles_SettleId",
				table: "AppUserSocialInfos");

			migrationBuilder.DropForeignKey(
				name: "FK_AppUserSocialInfoSettleMoments_AdminDivisions_AddressCode",
				table: "AppUserSocialInfoSettleMoments");

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

			migrationBuilder.DropPrimaryKey(
				name: "PK_AppUserSocialInfoSettleMoments",
				table: "AppUserSocialInfoSettleMoments");

			migrationBuilder.RenameTable(
				name: "AUserSocialInfoSettles",
				newName: "Settles");

			migrationBuilder.RenameTable(
				name: "AppUserSocialInfoSettleMoments",
				newName: "Moment");

			migrationBuilder.RenameIndex(
				name: "IX_AUserSocialInfoSettles_SelfId",
				table: "Settles",
				newName: "IX_Settles_SelfId");

			migrationBuilder.RenameIndex(
				name: "IX_AUserSocialInfoSettles_ParentId",
				table: "Settles",
				newName: "IX_Settles_ParentId");

			migrationBuilder.RenameIndex(
				name: "IX_AUserSocialInfoSettles_LoversParentId",
				table: "Settles",
				newName: "IX_Settles_LoversParentId");

			migrationBuilder.RenameIndex(
				name: "IX_AUserSocialInfoSettles_LoverId",
				table: "Settles",
				newName: "IX_Settles_LoverId");

			migrationBuilder.RenameIndex(
				name: "IX_AppUserSocialInfoSettleMoments_AddressCode",
				table: "Moment",
				newName: "IX_Moment_AddressCode");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Settles",
				table: "Settles",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Moment",
				table: "Moment",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_AppUserSocialInfos_Settles_SettleId",
				table: "AppUserSocialInfos",
				column: "SettleId",
				principalTable: "Settles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Moment_AdminDivisions_AddressCode",
				table: "Moment",
				column: "AddressCode",
				principalTable: "AdminDivisions",
				principalColumn: "Code",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Settles_Moment_LoverId",
				table: "Settles",
				column: "LoverId",
				principalTable: "Moment",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Settles_Moment_LoversParentId",
				table: "Settles",
				column: "LoversParentId",
				principalTable: "Moment",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Settles_Moment_ParentId",
				table: "Settles",
				column: "ParentId",
				principalTable: "Moment",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Settles_Moment_SelfId",
				table: "Settles",
				column: "SelfId",
				principalTable: "Moment",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}
	}
}