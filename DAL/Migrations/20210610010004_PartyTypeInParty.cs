using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class PartyTypeInParty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[,]
                {
                    { 42, "无", "#cccccc", "未指定面貌", "partyTypeInParty", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "none", 0 },
                    { 43, "群众", "#cccccc", "群众", "partyTypeInParty", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "social", 1 },
                    { 44, "少先队员", "#6bbade", "少先队员", "partyTypeInParty", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "child", 2 },
                    { 45, "共青团员", "#32e06f", "共青团员", "partyTypeInParty", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "teenager", 4 },
                    { 46, "入党积极分子", "#e28e8e", "入党积极分子", "partyTypeInParty", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "intend", 8 },
                    { 47, "预备党员", "#f53535", "预备党员", "partyTypeInParty", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ready", 16 },
                    { 48, "党员", "#e60000", "党员", "partyTypeInParty", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "finnally", 32 }
                });

            migrationBuilder.InsertData(
                table: "CommonDataGroups",
                columns: new[] { "Name", "Create", "Description", "Id", "IsRemoved", "IsRemovedDate" },
                values: new object[] { "partyTypeInParty", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "政治面貌", 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "CommonDataGroups",
                keyColumn: "Name",
                keyValue: "partyTypeInParty");
        }
    }
}
