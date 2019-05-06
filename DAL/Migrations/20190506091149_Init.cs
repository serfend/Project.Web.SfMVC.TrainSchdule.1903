using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminDivisions",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminDivisions", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "AppUserBaseInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RealName = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    PrivateAccount = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserBaseInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentCode = table.Column<string>(nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Companies_Companies_ParentCode",
                        column: x => x.ParentCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Duties",
                columns: table => new
                {
                    Code = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duties", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Regions = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplyRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StampLeave = table.Column<DateTime>(nullable: false),
                    StampReturn = table.Column<DateTime>(nullable: false),
                    OnTripLength = table.Column<int>(nullable: false),
                    VocationLength = table.Column<int>(nullable: false),
                    VocationType = table.Column<string>(nullable: true),
                    VocationPlaceCode = table.Column<int>(nullable: true),
                    Reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyRequests_AdminDivisions_VocationPlaceCode",
                        column: x => x.VocationPlaceCode,
                        principalTable: "AdminDivisions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSocialInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Settle = table.Column<int>(nullable: false),
                    AddressCode = table.Column<int>(nullable: true),
                    AddressDetail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSocialInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSocialInfo_AdminDivisions_AddressCode",
                        column: x => x.AddressCode,
                        principalTable: "AdminDivisions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCompanyInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyCode = table.Column<string>(nullable: true),
                    DutiesCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanyInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCompanyInfo_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCompanyInfo_Duties_DutiesCode",
                        column: x => x.DutiesCode,
                        principalTable: "Duties",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserApplicationInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InvitedBy = table.Column<string>(nullable: true),
                    PermissionId = table.Column<Guid>(nullable: true),
                    About = table.Column<string>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    AuthKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserApplicationInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserApplicationInfo_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<Guid>(nullable: true),
                    BaseInfoId = table.Column<Guid>(nullable: true),
                    CompanyInfoId = table.Column<Guid>(nullable: true),
                    SocialInfoId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_UserApplicationInfo_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "UserApplicationInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUsers_AppUserBaseInfos_BaseInfoId",
                        column: x => x.BaseInfoId,
                        principalTable: "AppUserBaseInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUsers_UserCompanyInfo_CompanyInfoId",
                        column: x => x.CompanyInfoId,
                        principalTable: "UserCompanyInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUsers_UserSocialInfo_SocialInfoId",
                        column: x => x.SocialInfoId,
                        principalTable: "UserSocialInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplyBaseInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RealName = table.Column<string>(nullable: true),
                    FromId = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    DutiesCode = table.Column<int>(nullable: true),
                    SocialId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyBaseInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyBaseInfos_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyBaseInfos_Duties_DutiesCode",
                        column: x => x.DutiesCode,
                        principalTable: "Duties",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyBaseInfos_AppUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyBaseInfos_UserSocialInfo_SocialId",
                        column: x => x.SocialId,
                        principalTable: "UserSocialInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BaseInfoId = table.Column<Guid>(nullable: true),
                    RequestInfoId = table.Column<Guid>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Hidden = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applies_ApplyBaseInfos_BaseInfoId",
                        column: x => x.BaseInfoId,
                        principalTable: "ApplyBaseInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applies_ApplyRequests_RequestInfoId",
                        column: x => x.RequestInfoId,
                        principalTable: "ApplyRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplyResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuditingById = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    HandleStamp = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ApplyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplyResponses_Applies_ApplyId",
                        column: x => x.ApplyId,
                        principalTable: "Applies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyResponses_AppUsers_AuditingById",
                        column: x => x.AuditingById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplyResponses_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applies_BaseInfoId",
                table: "Applies",
                column: "BaseInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_RequestInfoId",
                table: "Applies",
                column: "RequestInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_CompanyCode",
                table: "ApplyBaseInfos",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_DutiesCode",
                table: "ApplyBaseInfos",
                column: "DutiesCode");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_FromId",
                table: "ApplyBaseInfos",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyBaseInfos_SocialId",
                table: "ApplyBaseInfos",
                column: "SocialId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyRequests_VocationPlaceCode",
                table: "ApplyRequests",
                column: "VocationPlaceCode");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponses_ApplyId",
                table: "ApplyResponses",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponses_AuditingById",
                table: "ApplyResponses",
                column: "AuditingById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponses_CompanyCode",
                table: "ApplyResponses",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_ApplicationId",
                table: "AppUsers",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_BaseInfoId",
                table: "AppUsers",
                column: "BaseInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_CompanyInfoId",
                table: "AppUsers",
                column: "CompanyInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_SocialInfoId",
                table: "AppUsers",
                column: "SocialInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ParentCode",
                table: "Companies",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationInfo_PermissionId",
                table: "UserApplicationInfo",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyInfo_CompanyCode",
                table: "UserCompanyInfo",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyInfo_DutiesCode",
                table: "UserCompanyInfo",
                column: "DutiesCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialInfo_AddressCode",
                table: "UserSocialInfo",
                column: "AddressCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplyResponses");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Applies");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ApplyBaseInfos");

            migrationBuilder.DropTable(
                name: "ApplyRequests");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "UserApplicationInfo");

            migrationBuilder.DropTable(
                name: "AppUserBaseInfos");

            migrationBuilder.DropTable(
                name: "UserCompanyInfo");

            migrationBuilder.DropTable(
                name: "UserSocialInfo");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Duties");

            migrationBuilder.DropTable(
                name: "AdminDivisions");
        }
    }
}
