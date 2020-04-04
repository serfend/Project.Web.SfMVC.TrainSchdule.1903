using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class dutyType2 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "DutyTypes",
				columns: table => new
				{
					Code = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					DutiesCode = table.Column<int>(nullable: true),
					Name = table.Column<string>(nullable: true),
					AuditLevelNum = table.Column<int>(nullable: false),
					DutiesRawType = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_DutyTypes", x => x.Code);
					table.ForeignKey(
						name: "FK_DutyTypes_Duties_DutiesCode",
						column: x => x.DutiesCode,
						principalTable: "Duties",
						principalColumn: "Code",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_DutyTypes_DutiesCode",
				table: "DutyTypes",
				column: "DutiesCode");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "DutyTypes");
		}
	}
}