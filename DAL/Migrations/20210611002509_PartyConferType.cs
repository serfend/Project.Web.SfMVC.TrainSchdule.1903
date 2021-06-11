using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PartyConferType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PartyConferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[,]
                {
                    { 55, "党员发展", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", 0 },
                    { 56, "会议", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", 1 },
                    { 57, "党日", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", 2 },
                    { 58, "党课", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "4", 3 },
                    { 59, "谈心谈话", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "5", 4 },
                    { 60, "组织生活会", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6", 5 },
                    { 61, "党员汇报", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "7", 6 },
                    { 62, "民主评议党员", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8", 7 },
                    { 63, "思想政治教育", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "9", 8 },
                    { 64, "个人情况统计", "#333333", "", "PartyConferType", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10", 9 }
                });

            migrationBuilder.InsertData(
                table: "CommonDataGroups",
                columns: new[] { "Name", "Create", "Description", "Id", "IsRemoved", "IsRemovedDate" },
                values: new object[] { "PartyConferType", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "会议类型", 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "CommonDataGroups",
                keyColumn: "Name",
                keyValue: "PartyConferType");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PartyConferences");
        }
    }
}
