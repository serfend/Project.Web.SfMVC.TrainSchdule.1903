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
                name: "Filters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Alias = table.Column<string>(nullable: true),
                    Birth = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
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
                name: "ApplyResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuditingById = table.Column<Guid>(nullable: true),
                    CompanyId = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    HandleStamp = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    ApplyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyResponses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    ApplyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Companies_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Privilege = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    AuthKey = table.Column<string>(nullable: true),
                    RealName = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    About = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    WebSite = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    PrivateAccount = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DutiesId = table.Column<Guid>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    InvitedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AppUsers_Duties_DutiesId",
                        column: x => x.DutiesId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Applies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FromId = table.Column<Guid>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
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
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Applies_ApplyRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "ApplyRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Applies_ApplyStamps_stampId",
                        column: x => x.stampId,
                        principalTable: "ApplyStamps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BlackLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    BlockedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackLists_AppUsers_BlockedUserId",
                        column: x => x.BlockedUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BlackLists_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Confirmed",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    AdminId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confirmed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Confirmed_AppUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Confirmed_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Followings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    FollowedUserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Followings_AppUsers_FollowedUserId",
                        column: x => x.FollowedUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Followings_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermissionCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: true),
                    AuthBy = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionCompanies_AppUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    CountViews = table.Column<int>(nullable: false),
                    Manufacturer = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Iso = table.Column<int>(nullable: true),
                    Exposure = table.Column<double>(nullable: true),
                    Aperture = table.Column<double>(nullable: true),
                    FocalLength = table.Column<double>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: true),
                    FilterId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Filters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "Filters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Photos_AppUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    ReportedUserId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReports_AppUsers_ReportedUserId",
                        column: x => x.ReportedUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserReports_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    PhotoId = table.Column<Guid>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookmarks_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Bookmarks_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    PhotoId = table.Column<Guid>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AppUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Comments_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PhotoId = table.Column<Guid>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_AppUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Likes_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    PhotoId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoReports_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PhotoReports_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tagings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PhotoId = table.Column<Guid>(nullable: true),
                    TagId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tagings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tagings_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Tagings_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_ApplyResponses_ApplyId",
                table: "ApplyResponses",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponses_AuditingById",
                table: "ApplyResponses",
                column: "AuditingById");

            migrationBuilder.CreateIndex(
                name: "IX_ApplyResponses_CompanyId",
                table: "ApplyResponses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_CompanyId",
                table: "AppUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_DutiesId",
                table: "AppUsers",
                column: "DutiesId");

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
                name: "IX_BlackLists_BlockedUserId",
                table: "BlackLists",
                column: "BlockedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlackLists_UserId",
                table: "BlackLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_PhotoId",
                table: "Bookmarks",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_UserId",
                table: "Bookmarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_OwnerId",
                table: "Comments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PhotoId",
                table: "Comments",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ApplyId",
                table: "Companies",
                column: "ApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ParentId",
                table: "Companies",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Confirmed_AdminId",
                table: "Confirmed",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Confirmed_UserId",
                table: "Confirmed",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Followings_FollowedUserId",
                table: "Followings",
                column: "FollowedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Followings_UserId",
                table: "Followings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_OwnerId",
                table: "Likes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PhotoId",
                table: "Likes",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionCompanies_OwnerId",
                table: "PermissionCompanies",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoReports_PhotoId",
                table: "PhotoReports",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoReports_UserId",
                table: "PhotoReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_FilterId",
                table: "Photos",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_OwnerId",
                table: "Photos",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tagings_PhotoId",
                table: "Tagings",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tagings_TagId",
                table: "Tagings",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_ReportedUserId",
                table: "UserReports",
                column: "ReportedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_UserId",
                table: "UserReports",
                column: "UserId");

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
                name: "FK_ApplyResponses_Companies_CompanyId",
                table: "ApplyResponses",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Applies_ApplyId",
                table: "Companies",
                column: "ApplyId",
                principalTable: "Applies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applies_AppUsers_FromId",
                table: "Applies");

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
                name: "BlackLists");

            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Confirmed");

            migrationBuilder.DropTable(
                name: "Followings");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "PermissionCompanies");

            migrationBuilder.DropTable(
                name: "PhotoReports");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Tagings");

            migrationBuilder.DropTable(
                name: "UserReports");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Duties");

            migrationBuilder.DropTable(
                name: "Applies");

            migrationBuilder.DropTable(
                name: "ApplyRequests");

            migrationBuilder.DropTable(
                name: "ApplyStamps");
        }
    }
}
