using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplyRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    xjts = table.Column<int>(nullable: false),
                    ltts = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplyStamps",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ldsj = table.Column<DateTime>(nullable: false),
                    gdsj = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyStamps", x => x.Id);
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
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermittingAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermittingAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBaseInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthKey = table.Column<string>(nullable: true),
                    RealName = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    PrivateAccount = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBaseInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSocialInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    AddressDetail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSocialInfo", x => x.Id);
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
                    DutiesId = table.Column<Guid>(nullable: true)
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
                        name: "FK_UserCompanyInfo_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRange",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateId = table.Column<Guid>(nullable: true),
                    QueryId = table.Column<Guid>(nullable: true),
                    RemoveId = table.Column<Guid>(nullable: true),
                    ModifyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionRange_PermittingAction_CreateId",
                        column: x => x.CreateId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionRange_PermittingAction_ModifyId",
                        column: x => x.ModifyId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionRange_PermittingAction_QueryId",
                        column: x => x.QueryId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionRange_PermittingAction_RemoveId",
                        column: x => x.RemoveId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermittingAuth",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    AuthBy = table.Column<Guid>(nullable: false),
                    Create = table.Column<DateTime>(nullable: false),
                    PermittingActionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermittingAuth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermittingAuth_PermittingAction_PermittingActionId",
                        column: x => x.PermittingActionId,
                        principalTable: "PermittingAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Apply",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    申请信息Id = table.Column<Guid>(nullable: true),
                    审批流Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apply", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apply_PermissionRange_审批流Id",
                        column: x => x.审批流Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Apply_PermissionRange_申请信息Id",
                        column: x => x.申请信息Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    单位信息Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_PermissionRange_单位信息Id",
                        column: x => x.单位信息Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    基本信息Id = table.Column<Guid>(nullable: true),
                    社会关系Id = table.Column<Guid>(nullable: true),
                    职务信息Id = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_PermissionRange_基本信息Id",
                        column: x => x.基本信息Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_PermissionRange_社会关系Id",
                        column: x => x.社会关系Id,
                        principalTable: "PermissionRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_PermissionRange_职务信息Id",
                        column: x => x.职务信息Id,
                        principalTable: "PermissionRange",
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
                        name: "FK_ApplyResponses_Companies_CompanyCode",
                        column: x => x.CompanyCode,
                        principalTable: "Companies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    ApplyId = table.Column<Guid>(nullable: true),
                    CompanyId = table.Column<Guid>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Apply_ApplyId",
                        column: x => x.ApplyId,
                        principalTable: "Apply",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permissions_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
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
                        name: "FK_AppUsers_UserBaseInfo_BaseInfoId",
                        column: x => x.BaseInfoId,
                        principalTable: "UserBaseInfo",
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
                name: "Applies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromId = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    RequestId = table.Column<Guid>(nullable: true),
                    xjlb = table.Column<string>(nullable: true),
                    stampId = table.Column<Guid>(nullable: true),
                    Create = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Hidden = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applies_AppUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applies_ApplyRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "ApplyRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Applies_ApplyStamps_stampId",
                        column: x => x.stampId,
                        principalTable: "ApplyStamps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applies_FromId",
                table: "Applies",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_RequestId",
                table: "Applies",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Applies_stampId",
                table: "Applies",
                column: "stampId");

            migrationBuilder.CreateIndex(
                name: "IX_Apply_审批流Id",
                table: "Apply",
                column: "审批流Id");

            migrationBuilder.CreateIndex(
                name: "IX_Apply_申请信息Id",
                table: "Apply",
                column: "申请信息Id");

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
                name: "IX_Company_单位信息Id",
                table: "Company",
                column: "单位信息Id");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRange_CreateId",
                table: "PermissionRange",
                column: "CreateId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRange_ModifyId",
                table: "PermissionRange",
                column: "ModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRange_QueryId",
                table: "PermissionRange",
                column: "QueryId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRange_RemoveId",
                table: "PermissionRange",
                column: "RemoveId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ApplyId",
                table: "Permissions",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_CompanyId",
                table: "Permissions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_OwnerId",
                table: "Permissions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PermittingAuth_PermittingActionId",
                table: "PermittingAuth",
                column: "PermittingActionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_基本信息Id",
                table: "User",
                column: "基本信息Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_社会关系Id",
                table: "User",
                column: "社会关系Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_职务信息Id",
                table: "User",
                column: "职务信息Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserApplicationInfo_PermissionId",
                table: "UserApplicationInfo",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyInfo_CompanyCode",
                table: "UserCompanyInfo",
                column: "CompanyCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanyInfo_DutiesId",
                table: "UserCompanyInfo",
                column: "DutiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplyResponses_AppUsers_AuditingById",
                table: "ApplyResponses",
                column: "AuditingById",
                principalTable: "AppUsers",
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
                name: "FK_Permissions_AppUsers_OwnerId",
                table: "Permissions",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_AppUsers_OwnerId",
                table: "Permissions");

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
                name: "PermittingAuth");

            migrationBuilder.DropTable(
                name: "Applies");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ApplyRequests");

            migrationBuilder.DropTable(
                name: "ApplyStamps");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "UserApplicationInfo");

            migrationBuilder.DropTable(
                name: "UserBaseInfo");

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
                name: "Apply");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "PermissionRange");

            migrationBuilder.DropTable(
                name: "PermittingAction");
        }
    }
}
