using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class isettle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settle",
                table: "AppUserSocialInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "SettleId",
                table: "AppUserSocialInfos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Moment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Valid = table.Column<bool>(nullable: false),
                    AddressCode = table.Column<int>(nullable: true),
                    AddressDetail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Moment_AdminDivisions_AddressCode",
                        column: x => x.AddressCode,
                        principalTable: "AdminDivisions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Settle",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SelfId = table.Column<Guid>(nullable: true),
                    LoverId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settle_Moment_LoverId",
                        column: x => x.LoverId,
                        principalTable: "Moment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Settle_Moment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Moment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Settle_Moment_SelfId",
                        column: x => x.SelfId,
                        principalTable: "Moment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserSocialInfos_SettleId",
                table: "AppUserSocialInfos",
                column: "SettleId");

            migrationBuilder.CreateIndex(
                name: "IX_Moment_AddressCode",
                table: "Moment",
                column: "AddressCode");

            migrationBuilder.CreateIndex(
                name: "IX_Settle_LoverId",
                table: "Settle",
                column: "LoverId");

            migrationBuilder.CreateIndex(
                name: "IX_Settle_ParentId",
                table: "Settle",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Settle_SelfId",
                table: "Settle",
                column: "SelfId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserSocialInfos_Settle_SettleId",
                table: "AppUserSocialInfos",
                column: "SettleId",
                principalTable: "Settle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserSocialInfos_Settle_SettleId",
                table: "AppUserSocialInfos");

            migrationBuilder.DropTable(
                name: "Settle");

            migrationBuilder.DropTable(
                name: "Moment");

            migrationBuilder.DropIndex(
                name: "IX_AppUserSocialInfos_SettleId",
                table: "AppUserSocialInfos");

            migrationBuilder.DropColumn(
                name: "SettleId",
                table: "AppUserSocialInfos");

            migrationBuilder.AddColumn<int>(
                name: "Settle",
                table: "AppUserSocialInfos",
                nullable: false,
                defaultValue: 0);
        }
    }
}
