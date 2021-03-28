using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ApplyRequestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestType",
                table: "ApplyIndayRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VacationIndayTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermitCrossDay = table.Column<int>(type: "int", nullable: false),
                    NeedTrace = table.Column<bool>(type: "bit", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    Background = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionOnCompany = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationIndayTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "VacationIndayTypes",
                columns: new[] { "Id", "Alias", "Background", "Description", "Disabled", "IsRemoved", "IsRemovedDate", "Name", "NeedTrace", "PermitCrossDay", "RegionOnCompany" },
                values: new object[] { 1, "外出", "inday_outdoor.jpg", "办事、购物、休闲、一日内看病等利用非工作日的因私外出活动", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "外出", false, 0, "" });

            migrationBuilder.InsertData(
                table: "VacationIndayTypes",
                columns: new[] { "Id", "Alias", "Background", "Description", "Disabled", "IsRemoved", "IsRemovedDate", "Name", "NeedTrace", "PermitCrossDay", "RegionOnCompany" },
                values: new object[] { 2, "出差", "inday_business.jpg", "公差、接车/机、开会、保障等非集体活动外出", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "出差", false, 7, "" });

            migrationBuilder.InsertData(
                table: "VacationIndayTypes",
                columns: new[] { "Id", "Alias", "Background", "Description", "Disabled", "IsRemoved", "IsRemovedDate", "Name", "NeedTrace", "PermitCrossDay", "RegionOnCompany" },
                values: new object[] { 3, "回家", "inday_family.jpg", "工作日下班后、节假日全天等跨日外出", false, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "回家", false, 3, "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VacationIndayTypes");

            migrationBuilder.DropColumn(
                name: "RequestType",
                table: "ApplyIndayRequests");
        }
    }
}
