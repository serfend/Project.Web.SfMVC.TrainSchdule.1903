using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class DataDictionary_SeedExecute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommonDataDictionaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    GroupName = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonDataDictionaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonDataGroups",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    IsRemoved = table.Column<bool>(nullable: false),
                    IsRemovedDate = table.Column<DateTime>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonDataGroups", x => x.Name);
                });

            migrationBuilder.InsertData(
                table: "CommonDataDictionaries",
                columns: new[] { "Id", "Alias", "Color", "Description", "GroupName", "IsRemoved", "IsRemovedDate", "Key", "Value" },
                values: new object[,]
                {
                    { 1, "保存", "success", "保存到系统，使得记录不被删除", "ApplyAction", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Save", 2 },
                    { 17, "被召回", "#0000ffff", "因事提前归队", "ApplyExecuteStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Recall", 3 },
                    { 16, "推迟归队", "#ff0000ff", "因事推迟归队", "ApplyExecuteStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delay", 5 },
                    { 15, "已确认", "#008000ff", "已确认归队时间", "ApplyExecuteStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BeenSet", 1 },
                    { 14, "未确认", "#ff4500ff", "待确认归队时间", "ApplyExecuteStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NotSet", 0 },
                    { 13, "被作废", "#778899ff", "", "ApplyStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cancel", 120 },
                    { 12, "已通过", "#32cd32ff", "Cancel", "ApplyStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Accept", 100 },
                    { 10, "终审中", "#00bfffff", "Withdrew", "ApplyStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AcceptAndWaitAdmin", 50 },
                    { 11, "被驳回", "#ff0000ff", "", "ApplyStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Denied", 75 },
                    { 8, "已撤回", "#808080ff", "", "ApplyStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Withdrew", 20 },
                    { 7, "未发布", "#a9a9a9ff", "Publish##Delete", "ApplyStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NotPublish", 10 },
                    { 6, "未保存", "#000000ff", "Save##Publish##Delete", "ApplyStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NotSave", 0 },
                    { 5, "作废", "danger", "认定此次休假数据无效，且不可恢复", "ApplyAction", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cancel", 32 },
                    { 4, "撤回", "info", "取消审批流程，且不可恢复", "ApplyAction", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Withdrew", 16 },
                    { 3, "删除", "danger", "不再可见，且不可恢复", "ApplyAction", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delete", 8 },
                    { 2, "发布", "success", "开始审批流程。可以撤回申请，但不再可删除", "ApplyAction", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Publish", 4 },
                    { 9, "审核中", "#ff7f50ff", "Withdrew", "ApplyStatus", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Auditing", 40 }
                });

            migrationBuilder.InsertData(
                table: "CommonDataGroups",
                columns: new[] { "Name", "Create", "Description", "Id", "IsRemoved", "IsRemovedDate" },
                values: new object[,]
                {
                    { "ApplyAction", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "对应审批状态下可操作的行为。联动系统逻辑，勿修改", 3, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "ApplyStatus", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "休假申请的审批状态，其描述为可进行的操作名称。联动系统逻辑，勿修改", 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "ApplyAuditStatus", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "休假申请审批步骤的状态。联动系统逻辑，勿修改", 2, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "ApplyExecuteStatus", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "休假的落实状态。联动系统逻辑，勿修改", 4, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonDataDictionaries_Key",
                table: "CommonDataDictionaries",
                column: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonDataDictionaries");

            migrationBuilder.DropTable(
                name: "CommonDataGroups");
        }
    }
}
