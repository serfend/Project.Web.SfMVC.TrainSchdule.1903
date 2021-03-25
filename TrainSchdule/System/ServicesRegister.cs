using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using BLL.Interfaces.Audit;
using BLL.Interfaces.BBS;
using BLL.Interfaces.ClientDevice;
using BLL.Interfaces.Common;
using BLL.Interfaces.File;
using BLL.Interfaces.GameR3;
using BLL.Interfaces.IVacationStatistics;
using BLL.Interfaces.Permission;
using BLL.Interfaces.ZX;
using BLL.Interfaces.ZX.IGrade;
using BLL.Services;
using BLL.Services.ApplyServices;
using BLL.Services.ApplyServices.DailyApply;
using BLL.Services.Audit;
using BLL.Services.BBS;
using BLL.Services.ClientDevice;
using BLL.Services.Common;
using BLL.Services.File;
using BLL.Services.GameR3;
using BLL.Services.Permission;
using BLL.Services.VacationStatistics;
using BLL.Services.ZX;
using BLL.Services.ZX.Grade;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.System
{
	/// <summary>
	/// 服务注册中心
	/// </summary>
	public static class ServicesRegister
	{
		/// <summary>
		/// 注册服务
		/// </summary>
		/// <param name="services"></param>
		public static void RegisterServices(this IServiceCollection services)
		{
			services.RegisterServices_Common();
			services.RegisterServices_User();
			services.RegisterServices_Client();
			services.RegisterServices_Company();
			services.RegisterServices_Apply();
			services.RegisterServices_Grade();
			services.RegisterServices_R3();
		}

		/// <summary>
		/// 注册用户
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterServices_User(this IServiceCollection services)
		{
			services.AddScoped<IUsersService, UsersService>();
			services.AddScoped<IUserServiceDetail, UsersService>();
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddScoped<IUserActionServices, UserActionServices>();
		}

		/// <summary>
		/// 单位
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterServices_Company(this IServiceCollection services)
		{
			services.AddScoped<ICompaniesService, CompaniesService>();
			services.AddScoped<ICompanyManagerServices, CompanyManagerServices>();
		}

		/// <summary>
		/// R3
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterServices_R3(this IServiceCollection services)
		{
			services.AddScoped<IR3UsersServices, R3UsersServices>();
			services.AddScoped<IGameR3Services, R3HandleServices>();
		}

		/// <summary>
		/// 通用组件
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterServices_Common(this IServiceCollection services)
		{
			services.AddScoped<IEmailSender, EmailSender>();
			services.AddScoped<IGoogleAuthService, GoogleAuthService>();
			services.AddScoped<ISignInServices, SignInServices>();
			services.AddScoped<IFileServices, FileServices>();
			services.AddScoped<IDWZServices, DWZServices>();
			services.AddScoped<IDataDictionariesServices, DataDictionariesServices>();

			//单例
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton<IVerifyService, VerifyService>();
			services.AddSingleton<ICipperServices, CipperServices>();
			services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
			services.AddSingleton<IPermissionServices, PermissionServices>();
		}

		/// <summary>
		/// 休假
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterServices_Apply(this IServiceCollection services)
		{
			services.AddScoped<IApplyServiceCreate, ApplyServiceCreate>();
			services.AddScoped<IApplyVacationService, ApplyService>();
			services.AddScoped<IApplyInDayService, ApplyIndayService>();
			services.AddScoped<IRecallOrderServices, RecallOrderServices>();
			services.AddScoped<IVacationCheckServices, VacationCheckServices>();

			services.AddScoped<IStatisrticsAppliesServices, StatisrticsAppliesServices>();
			services.AddScoped<IStatisticsAppliesProcessServices, StatisticsAppliesProcessServices>();
			services.AddScoped<IStatisticsDailyProcessServices, StatisticsDailyProcessServices>();
			services.AddScoped<IApplyAuditStreamServices, ApplyAuditStreamRepositoryServices>();
			services.AddScoped<IAuditStreamServices, AuditStreamServices>();
		}

		/// <summary>
		/// 成绩
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterServices_Grade(this IServiceCollection services)
		{
			services.AddScoped<IPhyGradeServices, PhyGradeServices>();
			services.AddScoped<IGradeServices, GradeServices>();
		}
		/// <summary>
		/// 终端管控
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterServices_Client(this IServiceCollection services)
        {
			services.AddScoped<IClientVirusServices, ClientVirusServices>();
        }
	}
}