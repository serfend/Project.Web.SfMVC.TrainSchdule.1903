﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190505043157_admindivision")]
    partial class admindivision
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL.Entities.AdminDivision", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Code");

                    b.ToTable("AdminDivisions");
                });

            modelBuilder.Entity("DAL.Entities.Apply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Company");

                    b.Property<DateTime>("Create");

                    b.Property<string>("FromId");

                    b.Property<bool>("Hidden");

                    b.Property<string>("Reason");

                    b.Property<Guid?>("RequestId");

                    b.Property<int>("Status");

                    b.Property<Guid?>("stampId");

                    b.Property<string>("xjlb");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("RequestId");

                    b.HasIndex("stampId");

                    b.ToTable("Applies");
                });

            modelBuilder.Entity("DAL.Entities.ApplyRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ltts");

                    b.Property<int>("xjts");

                    b.HasKey("Id");

                    b.ToTable("ApplyRequests");
                });

            modelBuilder.Entity("DAL.Entities.ApplyResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplyId");

                    b.Property<string>("AuditingById");

                    b.Property<string>("CompanyCode");

                    b.Property<DateTime>("HandleStamp");

                    b.Property<string>("Remark");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ApplyId");

                    b.HasIndex("AuditingById");

                    b.HasIndex("CompanyCode");

                    b.ToTable("ApplyResponses");
                });

            modelBuilder.Entity("DAL.Entities.ApplyStamp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("gdsj");

                    b.Property<DateTime>("ldsj");

                    b.HasKey("Id");

                    b.ToTable("ApplyStamps");
                });

            modelBuilder.Entity("DAL.Entities.Duties", b =>
                {
                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Code");

                    b.ToTable("Duties");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Company", b =>
                {
                    b.Property<string>("Code");

                    b.Property<bool>("IsPrivate");

                    b.Property<string>("Name");

                    b.Property<string>("ParentCode");

                    b.HasKey("Code");

                    b.HasIndex("ParentCode");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.Apply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("审批流Id");

                    b.Property<Guid?>("申请信息Id");

                    b.HasKey("Id");

                    b.HasIndex("审批流Id");

                    b.HasIndex("申请信息Id");

                    b.ToTable("Apply");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("单位信息Id");

                    b.HasKey("Id");

                    b.HasIndex("单位信息Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.PermissionRange", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreateId");

                    b.Property<Guid?>("ModifyId");

                    b.Property<Guid?>("QueryId");

                    b.Property<Guid?>("RemoveId");

                    b.HasKey("Id");

                    b.HasIndex("CreateId");

                    b.HasIndex("ModifyId");

                    b.HasIndex("QueryId");

                    b.HasIndex("RemoveId");

                    b.ToTable("PermissionRange");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.Permissions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplyId");

                    b.Property<Guid?>("CompanyId");

                    b.Property<string>("OwnerId");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ApplyId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("UserId");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.PermittingAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("PermittingAction");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.PermittingAuth", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AuthBy");

                    b.Property<DateTime>("Create");

                    b.Property<string>("Path");

                    b.Property<Guid?>("PermittingActionId");

                    b.HasKey("Id");

                    b.HasIndex("PermittingActionId");

                    b.ToTable("PermittingAuth");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("基本信息Id");

                    b.Property<Guid?>("社会关系Id");

                    b.Property<Guid?>("职务信息Id");

                    b.HasKey("Id");

                    b.HasIndex("基本信息Id");

                    b.HasIndex("社会关系Id");

                    b.HasIndex("职务信息Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<Guid?>("ApplicationId");

                    b.Property<Guid?>("BaseInfoId");

                    b.Property<Guid?>("CompanyInfoId");

                    b.Property<Guid?>("SocialInfoId");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("BaseInfoId");

                    b.HasIndex("CompanyInfoId");

                    b.HasIndex("SocialInfoId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserApplicationInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<string>("AuthKey");

                    b.Property<DateTime>("Create");

                    b.Property<string>("Email");

                    b.Property<string>("InvitedBy");

                    b.Property<Guid?>("PermissionId");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.ToTable("UserApplicationInfo");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserBaseInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthKey");

                    b.Property<string>("Avatar");

                    b.Property<int>("Gender");

                    b.Property<bool>("PrivateAccount");

                    b.Property<string>("RealName");

                    b.HasKey("Id");

                    b.ToTable("UserBaseInfo");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserCompanyInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode");

                    b.Property<string>("DutiesCode");

                    b.HasKey("Id");

                    b.HasIndex("CompanyCode");

                    b.HasIndex("DutiesCode");

                    b.ToTable("UserCompanyInfo");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserSocialInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressCode");

                    b.Property<string>("AddressDetail");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.HasIndex("AddressCode");

                    b.ToTable("UserSocialInfo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DAL.Entities.Apply", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.User", "From")
                        .WithMany()
                        .HasForeignKey("FromId");

                    b.HasOne("DAL.Entities.ApplyRequest", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId");

                    b.HasOne("DAL.Entities.ApplyStamp", "stamp")
                        .WithMany()
                        .HasForeignKey("stampId");
                });

            modelBuilder.Entity("DAL.Entities.ApplyResponse", b =>
                {
                    b.HasOne("DAL.Entities.Apply")
                        .WithMany("Response")
                        .HasForeignKey("ApplyId");

                    b.HasOne("DAL.Entities.UserInfo.User", "AuditingBy")
                        .WithMany()
                        .HasForeignKey("AuditingById");

                    b.HasOne("DAL.Entities.UserInfo.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyCode");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Company", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Company", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentCode");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.Apply", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Permission.PermissionRange", "审批流")
                        .WithMany()
                        .HasForeignKey("审批流Id");

                    b.HasOne("DAL.Entities.UserInfo.Permission.PermissionRange", "申请信息")
                        .WithMany()
                        .HasForeignKey("申请信息Id");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.Company", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Permission.PermissionRange", "单位信息")
                        .WithMany()
                        .HasForeignKey("单位信息Id");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.PermissionRange", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Permission.PermittingAction", "Create")
                        .WithMany()
                        .HasForeignKey("CreateId");

                    b.HasOne("DAL.Entities.UserInfo.Permission.PermittingAction", "Modify")
                        .WithMany()
                        .HasForeignKey("ModifyId");

                    b.HasOne("DAL.Entities.UserInfo.Permission.PermittingAction", "Query")
                        .WithMany()
                        .HasForeignKey("QueryId");

                    b.HasOne("DAL.Entities.UserInfo.Permission.PermittingAction", "Remove")
                        .WithMany()
                        .HasForeignKey("RemoveId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.Permissions", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Permission.Apply", "Apply")
                        .WithMany()
                        .HasForeignKey("ApplyId");

                    b.HasOne("DAL.Entities.UserInfo.Permission.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");

                    b.HasOne("DAL.Entities.UserInfo.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("DAL.Entities.UserInfo.Permission.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.PermittingAuth", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Permission.PermittingAction")
                        .WithMany("PermittingAuths")
                        .HasForeignKey("PermittingActionId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.Permission.User", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Permission.PermissionRange", "基本信息")
                        .WithMany()
                        .HasForeignKey("基本信息Id");

                    b.HasOne("DAL.Entities.UserInfo.Permission.PermissionRange", "社会关系")
                        .WithMany()
                        .HasForeignKey("社会关系Id");

                    b.HasOne("DAL.Entities.UserInfo.Permission.PermissionRange", "职务信息")
                        .WithMany()
                        .HasForeignKey("职务信息Id");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.User", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.UserApplicationInfo", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.HasOne("DAL.Entities.UserInfo.UserBaseInfo", "BaseInfo")
                        .WithMany()
                        .HasForeignKey("BaseInfoId");

                    b.HasOne("DAL.Entities.UserInfo.UserCompanyInfo", "CompanyInfo")
                        .WithMany()
                        .HasForeignKey("CompanyInfoId");

                    b.HasOne("DAL.Entities.UserInfo.UserSocialInfo", "SocialInfo")
                        .WithMany()
                        .HasForeignKey("SocialInfoId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserApplicationInfo", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Permission.Permissions", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserCompanyInfo", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyCode");

                    b.HasOne("DAL.Entities.Duties", "Duties")
                        .WithMany()
                        .HasForeignKey("DutiesCode");
                });

            modelBuilder.Entity("DAL.Entities.UserInfo.UserSocialInfo", b =>
                {
                    b.HasOne("DAL.Entities.AdminDivision", "Address")
                        .WithMany()
                        .HasForeignKey("AddressCode");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DAL.Entities.UserInfo.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DAL.Entities.UserInfo.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
