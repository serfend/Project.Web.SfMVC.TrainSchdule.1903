using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PartyConferRecordContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyBaseConference_Companies_CreateByCode",
                table: "PartyBaseConference");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyConferWithTags_PartyBaseConference_ConferId",
                table: "PartyConferWithTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyUserRecords_PartyBaseConference_ConferenceId",
                table: "PartyUserRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartyBaseConference",
                table: "PartyBaseConference");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "PartyBaseConference");

            migrationBuilder.RenameTable(
                name: "PartyBaseConference",
                newName: "PartyConferences");

            migrationBuilder.RenameIndex(
                name: "IX_PartyBaseConference_CreateByCode",
                table: "PartyConferences",
                newName: "IX_PartyConferences_CreateByCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "Create",
                table: "PartyUserRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartyConferences",
                table: "PartyConferences",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PartyUserRecordContents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyUserRecordContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyUserRecordContents_PartyUserRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "PartyUserRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[,]
                {
                    { 49, "一般发言", "#333333", "发言或讲话", "PartyConferRecordType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", 0 },
                    { 50, "谈话人", "#333333", null, "PartyConferRecordType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", 1 },
                    { 51, "被谈话人", "#333333", null, "PartyConferRecordType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", 2 },
                    { 52, "推荐人", "#333333", null, "PartyConferRecordType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "4", 3 },
                    { 53, "介绍人", "#333333", null, "PartyConferRecordType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "5", 4 },
                    { 54, "学习笔记", "#333333", null, "PartyConferRecordType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6", 5 }
                });

            migrationBuilder.InsertData(
                table: "CommonDataGroups",
                columns: new[] { "Name", "Create", "Description", "Id", "IsRemoved", "IsRemovedDate" },
                values: new object[] { "PartyConferRecordType", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "会议记录内容的类型", 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_PartyUserRecordContents_RecordId",
                table: "PartyUserRecordContents",
                column: "RecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyConferences_Companies_CreateByCode",
                table: "PartyConferences",
                column: "CreateByCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyConferWithTags_PartyConferences_ConferId",
                table: "PartyConferWithTags",
                column: "ConferId",
                principalTable: "PartyConferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyUserRecords_PartyConferences_ConferenceId",
                table: "PartyUserRecords",
                column: "ConferenceId",
                principalTable: "PartyConferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyConferences_Companies_CreateByCode",
                table: "PartyConferences");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyConferWithTags_PartyConferences_ConferId",
                table: "PartyConferWithTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyUserRecords_PartyConferences_ConferenceId",
                table: "PartyUserRecords");

            migrationBuilder.DropTable(
                name: "PartyUserRecordContents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartyConferences",
                table: "PartyConferences");

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "CommonDataGroups",
                keyColumn: "Name",
                keyValue: "PartyConferRecordType");

            migrationBuilder.DropColumn(
                name: "Create",
                table: "PartyUserRecords");

            migrationBuilder.RenameTable(
                name: "PartyConferences",
                newName: "PartyBaseConference");

            migrationBuilder.RenameIndex(
                name: "IX_PartyConferences_CreateByCode",
                table: "PartyBaseConference",
                newName: "IX_PartyBaseConference_CreateByCode");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "PartyBaseConference",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartyBaseConference",
                table: "PartyBaseConference",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyBaseConference_Companies_CreateByCode",
                table: "PartyBaseConference",
                column: "CreateByCode",
                principalTable: "Companies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyConferWithTags_PartyBaseConference_ConferId",
                table: "PartyConferWithTags",
                column: "ConferId",
                principalTable: "PartyBaseConference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyUserRecords_PartyBaseConference_ConferenceId",
                table: "PartyUserRecords",
                column: "ConferenceId",
                principalTable: "PartyBaseConference",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
