using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ApplyIndayItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplyIndayId",
                table: "ApplyResponses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplyIndayId",
                table: "ApplyAuditSteps",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplyIndayRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StampLeave = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StampReturn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VacationPlaceCode = table.Column<int>(type: "int", nullable: true),
                    VacationPlaceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyIndayRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyIndayRequest_AdminDivisions_VacationPlaceCode",
                        column: x => x.VacationPlaceCode,
                        principalTable: "AdminDivisions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppliesInday",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Create = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BaseInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AuditLeader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainStatus = table.Column<int>(type: "int", nullable: false),
                    RecallId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplyAuditStreamSolutionRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NowAuditStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExecuteStatus = table.Column<int>(type: "int", nullable: false),
                    ExecuteStatusDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliesInday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppliesInday_ApplyAuditSteps_NowAuditStepId",
                        column: x => x.NowAuditStepId,
                        principalTable: "ApplyAuditSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppliesInday_ApplyAuditStreamSolutionRules_ApplyAuditStreamSolutionRuleId",
                        column: x => x.ApplyAuditStreamSolutionRuleId,
                        principalTable: "ApplyAuditStreamSolutionRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppliesInday_ApplyBaseInfos_BaseInfoId",
                        column: x => x.BaseInfoId,
                        principalTable: "ApplyBaseInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppliesInday_ApplyExcuteStatus_ExecuteStatusDetailId",
                        column: x => x.ExecuteStatusDetailId,
                        principalTable: "ApplyExcuteStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppliesInday_ApplyIndayRequest_RequestInfoId",
                        column: x => x.RequestInfoId,
                        principalTable: "ApplyIndayRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponses_ApplyIndayId",
                table: "ApplyResponses",
                column: "ApplyIndayId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyAuditSteps_ApplyIndayId",
                table: "ApplyAuditSteps",
                column: "ApplyIndayId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliesInday_ApplyAuditStreamSolutionRuleId",
                table: "AppliesInday",
                column: "ApplyAuditStreamSolutionRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliesInday_BaseInfoId",
                table: "AppliesInday",
                column: "BaseInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliesInday_ExecuteStatusDetailId",
                table: "AppliesInday",
                column: "ExecuteStatusDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliesInday_NowAuditStepId",
                table: "AppliesInday",
                column: "NowAuditStepId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliesInday_RequestInfoId",
                table: "AppliesInday",
                column: "RequestInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyIndayRequest_VacationPlaceCode",
                table: "ApplyIndayRequest",
                column: "VacationPlaceCode");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyAuditSteps_AppliesInday_ApplyIndayId",
                table: "ApplyAuditSteps",
                column: "ApplyIndayId",
                principalTable: "AppliesInday",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponses_AppliesInday_ApplyIndayId",
                table: "ApplyResponses",
                column: "ApplyIndayId",
                principalTable: "AppliesInday",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplyAuditSteps_AppliesInday_ApplyIndayId",
                table: "ApplyAuditSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyResponses_AppliesInday_ApplyIndayId",
                table: "ApplyResponses");

            migrationBuilder.DropTable(
                name: "AppliesInday");

            migrationBuilder.DropTable(
                name: "ApplyIndayRequest");

            migrationBuilder.DropIndex(
                name: "IX_ApplyResponses_ApplyIndayId",
                table: "ApplyResponses");

            migrationBuilder.DropIndex(
                name: "IX_ApplyAuditSteps_ApplyIndayId",
                table: "ApplyAuditSteps");

            migrationBuilder.DropColumn(
                name: "ApplyIndayId",
                table: "ApplyResponses");

            migrationBuilder.DropColumn(
                name: "ApplyIndayId",
                table: "ApplyAuditSteps");
        }
    }
}
