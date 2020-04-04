using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DAL.Migrations
{
	public partial class init : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AdminDivisions",
				columns: table => new
				{
					Code = table.Column<int>(nullable: false),
					ParentCode = table.Column<int>(nullable: false),
					Name = table.Column<string>(nullable: true),
					ShortName = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AdminDivisions", x => x.Code);
				});

			migrationBuilder.CreateTable(
				name: "AppUserApplicationSettings",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					LastSubmitApplyTime = table.Column<DateTime>(nullable: true),
					LastVocationUpdateTime = table.Column<DateTime>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppUserApplicationSettings", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AppUserBaseInfos",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Cid = table.Column<string>(nullable: false),
					RealName = table.Column<string>(nullable: true),
					Gender = table.Column<int>(nullable: false),
					Time_Work = table.Column<DateTime>(nullable: false),
					Time_BirthDay = table.Column<DateTime>(nullable: false),
					Time_Party = table.Column<DateTime>(nullable: false),
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
					IsPrivate = table.Column<bool>(nullable: false),
					Description = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Companies", x => x.Code);
				});

			migrationBuilder.CreateTable(
				name: "Duties",
				columns: table => new
				{
					Code = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(nullable: true),
					IsMajorManager = table.Column<bool>(nullable: false)
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
				name: "Subjects",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Name = table.Column<string>(nullable: true),
					ValueFormat = table.Column<int>(nullable: false),
					CountDown = table.Column<bool>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Subjects", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "TrainRank",
				columns: table => new
				{
					Code = table.Column<string>(nullable: false),
					Name = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TrainRank", x => x.Code);
				});

			migrationBuilder.CreateTable(
				name: "TrainType",
				columns: table => new
				{
					Code = table.Column<string>(nullable: false),
					Name = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_TrainType", x => x.Code);
				});

			migrationBuilder.CreateTable(
				name: "UserDiyInfo",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					About = table.Column<string>(nullable: true),
					Avatar = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserDiyInfo", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserTrainInfo",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserTrainInfo", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "VocationDescriptions",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Start = table.Column<DateTime>(nullable: false),
					Length = table.Column<int>(nullable: false),
					Name = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_VocationDescriptions", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ApplyRequests",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					StampLeave = table.Column<DateTime>(nullable: true),
					StampReturn = table.Column<DateTime>(nullable: true),
					OnTripLength = table.Column<int>(nullable: false),
					VocationLength = table.Column<int>(nullable: false),
					VocationType = table.Column<string>(nullable: true),
					VocationPlaceCode = table.Column<int>(nullable: true),
					Reason = table.Column<string>(nullable: true),
					CreateTime = table.Column<DateTime>(nullable: false),
					ByTransportation = table.Column<int>(nullable: false)
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
				name: "Moment",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Date = table.Column<DateTime>(nullable: false),
					Valid = table.Column<bool>(nullable: false),
					AddressCode = table.Column<int>(nullable: true),
					AddressDetail = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Moment", x => x.Id);
					table.ForeignKey(
						name: "FK_Moment_AdminDivisions_AddressCode",
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
				name: "AppUserCompanyInfos",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					CompanyCode = table.Column<string>(nullable: true),
					DutiesCode = table.Column<int>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppUserCompanyInfos", x => x.Id);
					table.ForeignKey(
						name: "FK_AppUserCompanyInfos_Companies_CompanyCode",
						column: x => x.CompanyCode,
						principalTable: "Companies",
						principalColumn: "Code",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AppUserCompanyInfos_Duties_DutiesCode",
						column: x => x.DutiesCode,
						principalTable: "Duties",
						principalColumn: "Code",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "AppUserApplicationInfos",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					InvitedBy = table.Column<string>(nullable: true),
					PermissionId = table.Column<Guid>(nullable: true),
					Create = table.Column<DateTime>(nullable: true),
					Email = table.Column<string>(nullable: true),
					AuthKey = table.Column<string>(nullable: true),
					ApplicationSettingId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppUserApplicationInfos", x => x.Id);
					table.ForeignKey(
						name: "FK_AppUserApplicationInfos_AppUserApplicationSettings_ApplicationSettingId",
						column: x => x.ApplicationSettingId,
						principalTable: "AppUserApplicationSettings",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AppUserApplicationInfos_Permissions_PermissionId",
						column: x => x.PermissionId,
						principalTable: "Permissions",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Standards",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					GradePairs = table.Column<string>(nullable: true),
					ExpressionWhenFullGrade = table.Column<string>(nullable: true),
					minAge = table.Column<int>(nullable: false),
					maxAge = table.Column<int>(nullable: false),
					gender = table.Column<int>(nullable: false),
					BaseStandard = table.Column<int>(nullable: false),
					SubjectId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Standards", x => x.Id);
					table.ForeignKey(
						name: "FK_Standards_Subjects_SubjectId",
						column: x => x.SubjectId,
						principalTable: "Subjects",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Train",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Time_Begin = table.Column<DateTime>(nullable: false),
					Time_End = table.Column<DateTime>(nullable: false),
					TrainName = table.Column<string>(nullable: true),
					TrainRankCode = table.Column<string>(nullable: true),
					TrainTypeCode = table.Column<string>(nullable: true),
					UserTrainInfoId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Train", x => x.Id);
					table.ForeignKey(
						name: "FK_Train_TrainRank_TrainRankCode",
						column: x => x.TrainRankCode,
						principalTable: "TrainRank",
						principalColumn: "Code",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Train_TrainType_TrainTypeCode",
						column: x => x.TrainTypeCode,
						principalTable: "TrainType",
						principalColumn: "Code",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Train_UserTrainInfo_UserTrainInfoId",
						column: x => x.UserTrainInfoId,
						principalTable: "UserTrainInfo",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "VocationAdditionals",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Name = table.Column<string>(nullable: true),
					Length = table.Column<int>(nullable: false),
					Start = table.Column<DateTime>(nullable: false),
					Description = table.Column<string>(nullable: true),
					ApplyRequestId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_VocationAdditionals", x => x.Id);
					table.ForeignKey(
						name: "FK_VocationAdditionals_ApplyRequests_ApplyRequestId",
						column: x => x.ApplyRequestId,
						principalTable: "ApplyRequests",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Settles",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					SelfId = table.Column<Guid>(nullable: true),
					LoverId = table.Column<Guid>(nullable: true),
					ParentId = table.Column<Guid>(nullable: true),
					LoversParentId = table.Column<Guid>(nullable: true),
					PrevYearlyLength = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Settles", x => x.Id);
					table.ForeignKey(
						name: "FK_Settles_Moment_LoverId",
						column: x => x.LoverId,
						principalTable: "Moment",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Settles_Moment_LoversParentId",
						column: x => x.LoversParentId,
						principalTable: "Moment",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Settles_Moment_ParentId",
						column: x => x.ParentId,
						principalTable: "Moment",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Settles_Moment_SelfId",
						column: x => x.SelfId,
						principalTable: "Moment",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "AppUserSocialInfos",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Phone = table.Column<string>(nullable: true),
					SettleId = table.Column<Guid>(nullable: true),
					AddressCode = table.Column<int>(nullable: true),
					AddressDetail = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppUserSocialInfos", x => x.Id);
					table.ForeignKey(
						name: "FK_AppUserSocialInfos_AdminDivisions_AddressCode",
						column: x => x.AddressCode,
						principalTable: "AdminDivisions",
						principalColumn: "Code",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AppUserSocialInfos_Settles_SettleId",
						column: x => x.SettleId,
						principalTable: "Settles",
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
					SocialInfoId = table.Column<Guid>(nullable: true),
					TrainInfoId = table.Column<Guid>(nullable: true),
					DiyInfoId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppUsers", x => x.Id);
					table.ForeignKey(
						name: "FK_AppUsers_AppUserApplicationInfos_ApplicationId",
						column: x => x.ApplicationId,
						principalTable: "AppUserApplicationInfos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AppUsers_AppUserBaseInfos_BaseInfoId",
						column: x => x.BaseInfoId,
						principalTable: "AppUserBaseInfos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AppUsers_AppUserCompanyInfos_CompanyInfoId",
						column: x => x.CompanyInfoId,
						principalTable: "AppUserCompanyInfos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AppUsers_UserDiyInfo_DiyInfoId",
						column: x => x.DiyInfoId,
						principalTable: "UserDiyInfo",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AppUsers_AppUserSocialInfos_SocialInfoId",
						column: x => x.SocialInfoId,
						principalTable: "AppUserSocialInfos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_AppUsers_UserTrainInfo_TrainInfoId",
						column: x => x.TrainInfoId,
						principalTable: "UserTrainInfo",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "ApplyBaseInfos",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					RealName = table.Column<string>(nullable: true),
					CompanyName = table.Column<string>(nullable: true),
					DutiesName = table.Column<string>(nullable: true),
					FromId = table.Column<string>(nullable: true),
					CompanyCode = table.Column<string>(nullable: true),
					DutiesCode = table.Column<int>(nullable: true),
					SocialId = table.Column<Guid>(nullable: true),
					CreateTime = table.Column<DateTime>(nullable: false)
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
						name: "FK_ApplyBaseInfos_AppUserSocialInfos_SocialId",
						column: x => x.SocialId,
						principalTable: "AppUserSocialInfos",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "CompanyManagers",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					UserId = table.Column<string>(nullable: true),
					CompanyCode = table.Column<string>(nullable: true),
					AuthById = table.Column<string>(nullable: true),
					Create = table.Column<DateTime>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CompanyManagers", x => x.Id);
					table.ForeignKey(
						name: "FK_CompanyManagers_AppUsers_AuthById",
						column: x => x.AuthById,
						principalTable: "AppUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_CompanyManagers_Companies_CompanyCode",
						column: x => x.CompanyCode,
						principalTable: "Companies",
						principalColumn: "Code",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_CompanyManagers_AppUsers_UserId",
						column: x => x.UserId,
						principalTable: "AppUsers",
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
					AuditLeader = table.Column<string>(nullable: true),
					Create = table.Column<DateTime>(nullable: true),
					Status = table.Column<int>(nullable: false),
					RecallId = table.Column<Guid>(nullable: false),
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
					HandleStamp = table.Column<DateTime>(nullable: true),
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

			migrationBuilder.CreateTable(
				name: "RecallOrders",
				columns: table => new
				{
					Id = table.Column<Guid>(nullable: false),
					Reason = table.Column<string>(nullable: true),
					RecallById = table.Column<string>(nullable: true),
					Create = table.Column<DateTime>(nullable: false),
					ReturnStramp = table.Column<DateTime>(nullable: false),
					ApplyId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RecallOrders", x => x.Id);
					table.ForeignKey(
						name: "FK_RecallOrders_Applies_ApplyId",
						column: x => x.ApplyId,
						principalTable: "Applies",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_RecallOrders_AppUsers_RecallById",
						column: x => x.RecallById,
						principalTable: "AppUsers",
						principalColumn: "Id",
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
				name: "IX_AppUserApplicationInfos_ApplicationSettingId",
				table: "AppUserApplicationInfos",
				column: "ApplicationSettingId");

			migrationBuilder.CreateIndex(
				name: "IX_AppUserApplicationInfos_PermissionId",
				table: "AppUserApplicationInfos",
				column: "PermissionId");

			migrationBuilder.CreateIndex(
				name: "IX_AppUserCompanyInfos_CompanyCode",
				table: "AppUserCompanyInfos",
				column: "CompanyCode");

			migrationBuilder.CreateIndex(
				name: "IX_AppUserCompanyInfos_DutiesCode",
				table: "AppUserCompanyInfos",
				column: "DutiesCode");

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
				name: "IX_AppUsers_DiyInfoId",
				table: "AppUsers",
				column: "DiyInfoId");

			migrationBuilder.CreateIndex(
				name: "IX_AppUsers_SocialInfoId",
				table: "AppUsers",
				column: "SocialInfoId");

			migrationBuilder.CreateIndex(
				name: "IX_AppUsers_TrainInfoId",
				table: "AppUsers",
				column: "TrainInfoId");

			migrationBuilder.CreateIndex(
				name: "IX_AppUserSocialInfos_AddressCode",
				table: "AppUserSocialInfos",
				column: "AddressCode");

			migrationBuilder.CreateIndex(
				name: "IX_AppUserSocialInfos_SettleId",
				table: "AppUserSocialInfos",
				column: "SettleId");

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
				name: "IX_CompanyManagers_AuthById",
				table: "CompanyManagers",
				column: "AuthById");

			migrationBuilder.CreateIndex(
				name: "IX_CompanyManagers_CompanyCode",
				table: "CompanyManagers",
				column: "CompanyCode");

			migrationBuilder.CreateIndex(
				name: "IX_CompanyManagers_UserId",
				table: "CompanyManagers",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Moment_AddressCode",
				table: "Moment",
				column: "AddressCode");

			migrationBuilder.CreateIndex(
				name: "IX_RecallOrders_ApplyId",
				table: "RecallOrders",
				column: "ApplyId");

			migrationBuilder.CreateIndex(
				name: "IX_RecallOrders_RecallById",
				table: "RecallOrders",
				column: "RecallById");

			migrationBuilder.CreateIndex(
				name: "IX_Settles_LoverId",
				table: "Settles",
				column: "LoverId");

			migrationBuilder.CreateIndex(
				name: "IX_Settles_LoversParentId",
				table: "Settles",
				column: "LoversParentId");

			migrationBuilder.CreateIndex(
				name: "IX_Settles_ParentId",
				table: "Settles",
				column: "ParentId");

			migrationBuilder.CreateIndex(
				name: "IX_Settles_SelfId",
				table: "Settles",
				column: "SelfId");

			migrationBuilder.CreateIndex(
				name: "IX_Standards_SubjectId",
				table: "Standards",
				column: "SubjectId");

			migrationBuilder.CreateIndex(
				name: "IX_Train_TrainRankCode",
				table: "Train",
				column: "TrainRankCode");

			migrationBuilder.CreateIndex(
				name: "IX_Train_TrainTypeCode",
				table: "Train",
				column: "TrainTypeCode");

			migrationBuilder.CreateIndex(
				name: "IX_Train_UserTrainInfoId",
				table: "Train",
				column: "UserTrainInfoId");

			migrationBuilder.CreateIndex(
				name: "IX_VocationAdditionals_ApplyRequestId",
				table: "VocationAdditionals",
				column: "ApplyRequestId");
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
				name: "CompanyManagers");

			migrationBuilder.DropTable(
				name: "RecallOrders");

			migrationBuilder.DropTable(
				name: "Standards");

			migrationBuilder.DropTable(
				name: "Train");

			migrationBuilder.DropTable(
				name: "VocationAdditionals");

			migrationBuilder.DropTable(
				name: "VocationDescriptions");

			migrationBuilder.DropTable(
				name: "AspNetRoles");

			migrationBuilder.DropTable(
				name: "AspNetUsers");

			migrationBuilder.DropTable(
				name: "Applies");

			migrationBuilder.DropTable(
				name: "Subjects");

			migrationBuilder.DropTable(
				name: "TrainRank");

			migrationBuilder.DropTable(
				name: "TrainType");

			migrationBuilder.DropTable(
				name: "ApplyBaseInfos");

			migrationBuilder.DropTable(
				name: "ApplyRequests");

			migrationBuilder.DropTable(
				name: "AppUsers");

			migrationBuilder.DropTable(
				name: "AppUserApplicationInfos");

			migrationBuilder.DropTable(
				name: "AppUserBaseInfos");

			migrationBuilder.DropTable(
				name: "AppUserCompanyInfos");

			migrationBuilder.DropTable(
				name: "UserDiyInfo");

			migrationBuilder.DropTable(
				name: "AppUserSocialInfos");

			migrationBuilder.DropTable(
				name: "UserTrainInfo");

			migrationBuilder.DropTable(
				name: "AppUserApplicationSettings");

			migrationBuilder.DropTable(
				name: "Permissions");

			migrationBuilder.DropTable(
				name: "Companies");

			migrationBuilder.DropTable(
				name: "Duties");

			migrationBuilder.DropTable(
				name: "Settles");

			migrationBuilder.DropTable(
				name: "Moment");

			migrationBuilder.DropTable(
				name: "AdminDivisions");
		}
	}
}