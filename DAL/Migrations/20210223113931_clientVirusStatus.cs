using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class clientVirusStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Alias", "Description", "GroupName" },
                values: new object[] { "待处理", "处于待处理状态", "clientVirusStatus" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Alias", "Description", "GroupName", "Key", "Value" },
                values: new object[] { "处置成功", "此项已处置成功", "clientVirusStatus", "Success", 1 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Alias", "Description", "GroupName", "Key", "Value" },
                values: new object[] { "终端推送已发出", "已通过推送系统向终端发送染毒通告待处理中", "clientVirusStatus", "ClientNotify", 2 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Alias", "Description", "GroupName", "Key", "Value" },
                values: new object[] { "第三方消息已发出", "已通过第三方系统发布消息", "clientVirusStatus", "MessageSend", 4 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "无", "暂无状态", "None", 0 });

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
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "新增待处置", "终端设备新增待处置", "ClientDeviceVirusNew", 416 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "处置成功", "终端设备新增处置成功", "ClientDeviceVirusNewSuccess", 417 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "新增未处置", "终端设备新增未处置", "ClientDeviceVirusNewUnhandle", 418 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "新增处置失败", "终端设备新增处置失败", "ClientDeviceVirusNewFail", 419 });

            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[,]
                {
                    { 29, "新增已处置", "#d3d3d3ff", "终端设备新增已处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandle", 448 },
                    { 30, "自主处置", "#d3d3d3ff", "自主处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleByUser", 449 },
                    { 31, "第三方处置", "#d3d3d3ff", "通过第三方处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleByIgnore", 461 },
                    { 32, "提交处置", "#d3d3d3ff", "通过提交方式处置", "clientVirusHandleStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ClientDeviceVirusHandleBySubmit", 481 }
                });

            migrationBuilder.InsertData(
                table: "CommonDataGroups",
                columns: new[] { "Name", "Create", "Description", "Id", "IsRemoved", "IsRemovedDate" },
                values: new object[] { "clientVirusStatus", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "病毒状态，不应修改", 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "CommonDataGroups",
                keyColumn: "Name",
                keyValue: "clientVirusStatus");

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Alias", "Description", "GroupName" },
                values: new object[] { "无", "暂无状态", "clientVirusHandleStatus" });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Alias", "Description", "GroupName", "Key", "Value" },
                values: new object[] { "终端设备", "终端设备", "clientVirusHandleStatus", "ClientDevice", 256 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Alias", "Description", "GroupName", "Key", "Value" },
                values: new object[] { "终端病毒", "终端设备病毒", "clientVirusHandleStatus", "ClientDeviceVirus", 384 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Alias", "Description", "GroupName", "Key", "Value" },
                values: new object[] { "新增待处置", "终端设备新增待处置", "clientVirusHandleStatus", "ClientDeviceVirusNew", 416 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "处置成功", "终端设备新增处置成功", "ClientDeviceVirusNewSuccess", 417 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "新增未处置", "终端设备新增未处置", "ClientDeviceVirusNewUnhandle", 418 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "新增处置失败", "终端设备新增处置失败", "ClientDeviceVirusNewFail", 419 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "新增已处置", "终端设备新增已处置", "ClientDeviceVirusHandle", 448 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "自主处置", "自主处置", "ClientDeviceVirusHandleByUser", 449 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "第三方处置", "通过第三方处置", "ClientDeviceVirusHandleByIgnore", 461 });

            migrationBuilder.UpdateData(
                table: "CommonDataDictionaries",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Alias", "Description", "Key", "Value" },
                values: new object[] { "提交处置", "通过提交方式处置", "ClientDeviceVirusHandleBySubmit", 481 });
        }
    }
}
