using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class VirusHandleStatusDict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[,]
                {
                    { 18, "无", "#d3d3d3ff", "暂无状态", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "None", 0 },
                    { 19, "终端设备", "#d3d3d3ff", "终端设备", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDevice", 256 },
                    { 20, "终端病毒", "#d3d3d3ff", "终端设备病毒", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirus", 384 },
                    { 21, "新增待处置", "#d3d3d3ff", "终端设备新增待处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusNew", 416 },
                    { 22, "处置成功", "#d3d3d3ff", "终端设备新增处置成功", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusNewSuccess", 417 },
                    { 23, "新增未处置", "#d3d3d3ff", "终端设备新增未处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusNewUnhandle", 418 },
                    { 24, "新增处置失败", "#d3d3d3ff", "终端设备新增处置失败", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusNewFail", 419 },
                    { 25, "新增已处置", "#d3d3d3ff", "终端设备新增已处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandle", 448 },
                    { 26, "自主处置", "#d3d3d3ff", "自主处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleByUser", 449 },
                    { 27, "第三方处置", "#d3d3d3ff", "通过第三方处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleByIgnore", 461 },
                    { 28, "提交处置", "#d3d3d3ff", "通过提交方式处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleBySubmit", 481 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 28);
        }
    }
}
