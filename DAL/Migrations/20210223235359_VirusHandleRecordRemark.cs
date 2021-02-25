using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class VirusHandleRecordRemark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "VirusHandleRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "染毒通告", "#ffdab9ff", "通过公告系统发出染毒通告", "ClientDeviceVirusNotify", 388 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "染毒即时消息", "#9370dbff", "通过第三方发出染毒即时消息", "ClientDeviceVirusMessage", 389 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增待处置", "#ff0000ff", "终端设备新增待处置", "ClientDeviceVirusNew", 416 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "处置成功", "#228b22ff", "终端设备新增处置成功", "ClientDeviceVirusNewSuccess", 417 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增未处置", "#8b0000ff", "终端设备新增未处置", "ClientDeviceVirusNewUnhandle", 418 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增处置失败", "#cd5c5cff", "终端设备新增处置失败", "ClientDeviceVirusNewFail", 419 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增已处置", "#f0f8ffff", "终端设备新增已处置", "ClientDeviceVirusHandle", 448 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "自主处置", "#f0f8ffff", "自主处置", "ClientDeviceVirusHandleByUser", 449 });

            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[,]
                {
                    { 34, "第三方处置", "#1e90ffff", "通过第三方处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleByIgnore", 461 },
                    { 35, "提交处置", "#0000cdff", "通过提交方式处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleBySubmit", 481 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "VirusHandleRecords");

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增待处置", "#ff0000ff", "终端设备新增待处置", "ClientDeviceVirusNew", 416 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "处置成功", "#228b22ff", "终端设备新增处置成功", "ClientDeviceVirusNewSuccess", 417 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增未处置", "#8b0000ff", "终端设备新增未处置", "ClientDeviceVirusNewUnhandle", 418 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增处置失败", "#cd5c5cff", "终端设备新增处置失败", "ClientDeviceVirusNewFail", 419 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增已处置", "#f0f8ffff", "终端设备新增已处置", "ClientDeviceVirusHandle", 448 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "自主处置", "#f0f8ffff", "自主处置", "ClientDeviceVirusHandleByUser", 449 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "第三方处置", "#1e90ffff", "通过第三方处置", "ClientDeviceVirusHandleByIgnore", 461 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "提交处置", "#0000cdff", "通过提交方式处置", "ClientDeviceVirusHandleBySubmit", 481 });
        }
    }
}
