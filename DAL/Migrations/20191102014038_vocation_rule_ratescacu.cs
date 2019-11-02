using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class vocation_rule_ratescacu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserSocialInfos_Settle_SettleId",
                table: "AppUserSocialInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Settle_Moment_LoverId",
                table: "Settle");

            migrationBuilder.DropForeignKey(
                name: "FK_Settle_Moment_ParentId",
                table: "Settle");

            migrationBuilder.DropForeignKey(
                name: "FK_Settle_Moment_SelfId",
                table: "Settle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Settle",
                table: "Settle");

            migrationBuilder.RenameTable(
                name: "Settle",
                newName: "Settles");

            migrationBuilder.RenameIndex(
                name: "IX_Settle_SelfId",
                table: "Settles",
                newName: "IX_Settles_SelfId");

            migrationBuilder.RenameIndex(
                name: "IX_Settle_ParentId",
                table: "Settles",
                newName: "IX_Settles_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Settle_LoverId",
                table: "Settles",
                newName: "IX_Settles_LoverId");

            migrationBuilder.AddColumn<int>(
                name: "PrevYearlyLength",
                table: "Settles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settles",
                table: "Settles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserSocialInfos_Settles_SettleId",
                table: "AppUserSocialInfos",
                column: "SettleId",
                principalTable: "Settles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Settles_Moment_LoverId",
                table: "Settles",
                column: "LoverId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserSocialInfos_Settles_SettleId",
                table: "AppUserSocialInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_Settles_Moment_LoverId",
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

            migrationBuilder.DropColumn(
                name: "PrevYearlyLength",
                table: "Settles");

            migrationBuilder.RenameTable(
                name: "Settles",
                newName: "Settle");

            migrationBuilder.RenameIndex(
                name: "IX_Settles_SelfId",
                table: "Settle",
                newName: "IX_Settle_SelfId");

            migrationBuilder.RenameIndex(
                name: "IX_Settles_ParentId",
                table: "Settle",
                newName: "IX_Settle_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Settles_LoverId",
                table: "Settle",
                newName: "IX_Settle_LoverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Settle",
                table: "Settle",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserSocialInfos_Settle_SettleId",
                table: "AppUserSocialInfos",
                column: "SettleId",
                principalTable: "Settle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Settle_Moment_LoverId",
                table: "Settle",
                column: "LoverId",
                principalTable: "Moment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Settle_Moment_ParentId",
                table: "Settle",
                column: "ParentId",
                principalTable: "Moment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Settle_Moment_SelfId",
                table: "Settle",
                column: "SelfId",
                principalTable: "Moment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
