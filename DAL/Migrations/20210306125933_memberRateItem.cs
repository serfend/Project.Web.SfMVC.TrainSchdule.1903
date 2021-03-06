using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class memberRateItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[,]
                {
                    { 36, "不称职", "#e60039", "没有达到岗位基本要求，需备注不称职原因", "NormalRateLevel", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L1", 100 },
                    { 37, "较差", "#c85554", "基本达到岗位要求", "NormalRateLevel", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L2", 300 },
                    { 38, "称职", "#337d56", "完全达到本职岗位的所有要求", "NormalRateLevel", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L3", 500 },
                    { 39, "良好", "#9ec8da", "达到并高于本岗位所有要求的标准", "NormalRateLevel", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L4", 700 },
                    { 40, "优秀", "#6ff9c1", "表现突出，大幅超出当前岗位要求。需备注优秀原因。", "NormalRateLevel", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L5", 900 },
                    { 41, "无", "#cbe2e4", "未选择", "NormalRateLevel", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "None", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 41);
        }
    }
}
