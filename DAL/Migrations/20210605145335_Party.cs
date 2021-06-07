using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Party : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserOrderRank",
                table: "AppUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PartyBaseConference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyBaseConference", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartyDuties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Enable = table.Column<bool>(type: "bit", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyDuties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartyGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChairmanId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ViceChairmanId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GroupType = table.Column<int>(type: "int", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyGroups_AppUsers_ChairmanId",
                        column: x => x.ChairmanId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyGroups_AppUsers_ViceChairmanId",
                        column: x => x.ViceChairmanId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyGroups_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyUserRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ConferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyUserRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyUserRecords_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PartyUserRecords_PartyBaseConference_ConferenceId",
                        column: x => x.ConferenceId,
                        principalTable: "PartyBaseConference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPartyInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DutyId = table.Column<int>(type: "int", nullable: false),
                    DutyStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeInParty = table.Column<int>(type: "int", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PartyGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPartyInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPartyInfos_AppUsers_userName",
                        column: x => x.userName,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPartyInfos_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPartyInfos_PartyDuties_DutyId",
                        column: x => x.DutyId,
                        principalTable: "PartyDuties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPartyInfos_PartyGroups_PartyGroupId",
                        column: x => x.PartyGroupId,
                        principalTable: "PartyGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartyGroups_ChairmanId",
                table: "PartyGroups",
                column: "ChairmanId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyGroups_CompanyCode",
                table: "PartyGroups",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_PartyGroups_ViceChairmanId",
                table: "PartyGroups",
                column: "ViceChairmanId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyUserRecords_ConferenceId",
                table: "PartyUserRecords",
                column: "ConferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyUserRecords_UserId",
                table: "PartyUserRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyInfos_CompanyCode",
                table: "UserPartyInfos",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyInfos_DutyId",
                table: "UserPartyInfos",
                column: "DutyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyInfos_PartyGroupId",
                table: "UserPartyInfos",
                column: "PartyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPartyInfos_userName",
                table: "UserPartyInfos",
                column: "userName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartyUserRecords");

            migrationBuilder.DropTable(
                name: "UserPartyInfos");

            migrationBuilder.DropTable(
                name: "PartyBaseConference");

            migrationBuilder.DropTable(
                name: "PartyDuties");

            migrationBuilder.DropTable(
                name: "PartyGroups");

            migrationBuilder.DropColumn(
                name: "UserOrderRank",
                table: "AppUsers");
        }
    }
}
