using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ClientVirusStatusEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Alias", "Description" },
                values: new object[] { "无状态", "无状态可用" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Alias", "Color", "Description", "Key" },
                values: new object[] { "待处理", "#ff0000ff", "处于待处理状态", "Unhandle" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Alias", "Color", "Description", "Key" },
                values: new object[] { "处置成功", "#228b22ff", "此项已处置成功", "Success" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Alias", "Description", "Key" },
                values: new object[] { "终端推送已发出", "已通过推送系统向终端发送染毒通告待处理中", "ClientNotify" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Alias", "Description", "GroupName", "Key", "Value" },
                values: new object[] { "第三方消息已发出", "已通过第三方系统发布消息", "clientVirusStatus", "MessageSend", 8 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "无", "暂无状态", "None", 0 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "终端设备", "终端设备", "ClientDevice", 256 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "终端病毒", "#d3d3d3ff", "终端设备病毒", "ClientDeviceVirus", 384 });

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
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "新增已处置", "终端设备新增已处置", "ClientDeviceVirusHandle", 448 });

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

            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[] { 33, "提交处置", "#0000cdff", "通过提交方式处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleBySubmit", 481 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Alias", "Description" },
                values: new object[] { "待处理", "处于待处理状态" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Alias", "Color", "Description", "Key" },
                values: new object[] { "处置成功", "#228b22ff", "此项已处置成功", "Success" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Alias", "Color", "Description", "Key" },
                values: new object[] { "终端推送已发出", "#d3d3d3ff", "已通过推送系统向终端发送染毒通告待处理中", "ClientNotify" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Alias", "Description", "Key" },
                values: new object[] { "第三方消息已发出", "已通过第三方系统发布消息", "MessageSend" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Alias", "Description", "GroupName", "Key", "Value" },
                values: new object[] { "无", "暂无状态", "clientVirusHandleStatus", "None", 0 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "终端设备", "终端设备", "ClientDevice", 256 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "终端病毒", "终端设备病毒", "ClientDeviceVirus", 384 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增待处置", "#ff0000ff", "终端设备新增待处置", "ClientDeviceVirusNew", 416 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "处置成功", "#228b22ff", "终端设备新增处置成功", "ClientDeviceVirusNewSuccess", 417 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增未处置", "#8b0000ff", "终端设备新增未处置", "ClientDeviceVirusNewUnhandle", 418 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增处置失败", "#cd5c5cff", "终端设备新增处置失败", "ClientDeviceVirusNewFail", 419 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "新增已处置", "#f0f8ffff", "终端设备新增已处置", "ClientDeviceVirusHandle", 448 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "自主处置", "自主处置", "ClientDeviceVirusHandleByUser", 449 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "第三方处置", "#1e90ffff", "通过第三方处置", "ClientDeviceVirusHandleByIgnore", 461 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Alias", "Color", "Description", "Key", "Value" },
                values: new object[] { "提交处置", "#0000cdff", "通过提交方式处置", "ClientDeviceVirusHandleBySubmit", 481 });
        }
    }
}
