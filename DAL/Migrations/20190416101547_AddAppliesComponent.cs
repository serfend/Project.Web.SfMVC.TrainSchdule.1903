using Microsoft.EntityFrameworkCore.Migrations;

namespace TrainSchdule.DAL.Migrations
{
    public partial class AddAppliesComponent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyRequest_RequestId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyStamp_stampId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyResponse_Applies_ApplyId",
                table: "ApplyResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyResponse_AppUsers_AuditingById",
                table: "ApplyResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyResponse_Companies_CompanyId",
                table: "ApplyResponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyStamp",
                table: "ApplyStamp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyResponse",
                table: "ApplyResponse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyRequest",
                table: "ApplyRequest");

            migrationBuilder.RenameTable(
                name: "ApplyStamp",
                newName: "ApplyStamps");

            migrationBuilder.RenameTable(
                name: "ApplyResponse",
                newName: "ApplyResponses");

            migrationBuilder.RenameTable(
                name: "ApplyRequest",
                newName: "ApplyRequests");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyResponse_CompanyId",
                table: "ApplyResponses",
                newName: "IX_ApplyResponses_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyResponse_AuditingById",
                table: "ApplyResponses",
                newName: "IX_ApplyResponses_AuditingById");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyResponse_ApplyId",
                table: "ApplyResponses",
                newName: "IX_ApplyResponses_ApplyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyStamps",
                table: "ApplyStamps",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyResponses",
                table: "ApplyResponses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyRequests",
                table: "ApplyRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyRequests_RequestId",
                table: "Applies",
                column: "RequestId",
                principalTable: "ApplyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyStamps_stampId",
                table: "Applies",
                column: "stampId",
                principalTable: "ApplyStamps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponses_Applies_ApplyId",
                table: "ApplyResponses",
                column: "ApplyId",
                principalTable: "Applies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponses_AppUsers_AuditingById",
                table: "ApplyResponses",
                column: "AuditingById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponses_Companies_CompanyId",
                table: "ApplyResponses",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyRequests_RequestId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_Applies_ApplyStamps_stampId",
                table: "Applies");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyResponses_Applies_ApplyId",
                table: "ApplyResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyResponses_AppUsers_AuditingById",
                table: "ApplyResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplyResponses_Companies_CompanyId",
                table: "ApplyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyStamps",
                table: "ApplyStamps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyResponses",
                table: "ApplyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplyRequests",
                table: "ApplyRequests");

            migrationBuilder.RenameTable(
                name: "ApplyStamps",
                newName: "ApplyStamp");

            migrationBuilder.RenameTable(
                name: "ApplyResponses",
                newName: "ApplyResponse");

            migrationBuilder.RenameTable(
                name: "ApplyRequests",
                newName: "ApplyRequest");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyResponses_CompanyId",
                table: "ApplyResponse",
                newName: "IX_ApplyResponse_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyResponses_AuditingById",
                table: "ApplyResponse",
                newName: "IX_ApplyResponse_AuditingById");

            migrationBuilder.RenameIndex(
                name: "IX_ApplyResponses_ApplyId",
                table: "ApplyResponse",
                newName: "IX_ApplyResponse_ApplyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyStamp",
                table: "ApplyStamp",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyResponse",
                table: "ApplyResponse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplyRequest",
                table: "ApplyRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyRequest_RequestId",
                table: "Applies",
                column: "RequestId",
                principalTable: "ApplyRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applies_ApplyStamp_stampId",
                table: "Applies",
                column: "stampId",
                principalTable: "ApplyStamp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponse_Applies_ApplyId",
                table: "ApplyResponse",
                column: "ApplyId",
                principalTable: "Applies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponse_AppUsers_AuditingById",
                table: "ApplyResponse",
                column: "AuditingById",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponse_Companies_CompanyId",
                table: "ApplyResponse",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
