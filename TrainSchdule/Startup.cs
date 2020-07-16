using System;
using System.IO;
using BLL.Crontab;
using BLL.Services.Common;
using BLL.Services.File;
using DAL.Data;
using DAL.Entities.UserInfo;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TrainSchdule.Controllers.Log;
using TrainSchdule.Crontab;
using TrainSchdule.System;

namespace TrainSchdule
{
	/// <summary>
	///
	/// </summary>
	public class Startup
	{
		#region Properties

		/// <summary>
		///
		/// </summary>
		public IConfiguration Configuration { get; set; }

		private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

		#endregion Properties

		#region .ctors

		/// <summary>
		///
		/// </summary>
		/// <param name="configuration"></param>
		public Startup(IConfiguration configuration)
		{
			//注入Configuration服务
			Configuration = configuration;
		}

		#endregion .ctors

		#region Logic

		private void AddApplicationServices(IServiceCollection services)
		{
			services.RegisterServices();
		}

		private void AddHangfireServices(IServiceCollection services)
		{
			// Add Hangfire services.
			services.AddHangfire(configuration => configuration
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
				{
					CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
					SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
					QueuePollInterval = TimeSpan.Zero,
					UseRecommendedIsolationLevel = true,
					UsePageLocksOnDequeue = true,
					DisableGlobalLocks = true
				}));

			// Add the processing server as IHostedService
			services.AddHangfireServer();
		}

		private void ConfigureHangfireServices()
		{
			RecurringJob.AddOrUpdate<ApplyClearJob>((a) => a.Run("OnJob"), Cron.Daily);
			RecurringJob.AddOrUpdate<UserInfoClearJob>((a) => a.Run(), Cron.Hourly);
			RecurringJob.AddOrUpdate<FileServices>((u) => u.RemoveTimeoutUploadStatus(), Cron.Hourly);
			BackgroundJob.Enqueue<ApplyClearJob>((a) => a.Run("OnStart"));
			BackgroundJob.Enqueue<UserInfoClearJob>((a) => a.Run());
			BackgroundJob.Enqueue<FileServices>((a) => a.RemoveTimeoutUploadStatus());
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				var connectionString = Configuration.GetConnectionString("DefaultConnection");
				options.UseLazyLoadingProxies()
					   .UseSqlServer(connectionString);
			});
			AddAllowCorsServices(services);

			AddHangfireServices(services);
			AddSwaggerServices(services);
			services.Configure<IdentityOptions>(options =>
			{
				// Password settings
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = false;
				options.Password.RequiredUniqueChars = 4;

				// Lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
				options.Lockout.MaxFailedAccessAttempts = 10;
				options.Lockout.AllowedForNewUsers = true;

				// User settings
				options.User.RequireUniqueEmail = true;
			});
			AddApplicationServices(services);
			services.AddMvc(option =>
			{
				option.Filters.Add<ActionStatusMessageExceptionFilter>();
			}).AddJsonOptions(opt => opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
			//.AddJsonOptions(opt => opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
			// TODO use yyyy-mm-ddThh:ii:ssZ (UTC)
		}

		private void AddSwaggerServices(IServiceCollection services)
		{
			//注册Swagger生成器，定义一个和多个Swagger 文档
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info { Title = "TrainSchdule", Version = "v1" });
				// 为 Swagger JSON and UI设置xml文档注释路径
				var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
				var xmlPath = Path.Combine(basePath, "TrainSchdule.xml");
				c.IncludeXmlComments(xmlPath, true);
			});
		}

		private void AddAllowCorsServices(IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(MyAllowSpecificOrigins,
					builder =>
					{
						builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((x) => true);
					});
			});

			services.Configure<CookiePolicyOptions>(options =>
			{
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			services.ConfigureExternalCookie(options =>
			{
				// Other options
				options.Cookie.SameSite = SameSiteMode.None;
			});
			services.ConfigureApplicationCookie(options =>
			{
				// Other options
				options.Cookie.SameSite = SameSiteMode.None;
			});
			services.AddAuthentication()
				.AddCookie(options =>
				{
					options.Cookie.SameSite = SameSiteMode.None;
				});
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			services.AddSession(s =>
			{
				s.IdleTimeout = TimeSpan.FromMinutes(60);
				s.Cookie.SameSite = SameSiteMode.None;
			});
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="services"></param>
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
		{
			if (env.IsDevelopment())
			{
			}
			else if (env.IsProduction())
			{
			}
			app.Map("/ws/log", WebSocketLog.Map);// 连接到资源上报的ws
			app.UseHangfireServer();
			app.UseHangfireDashboard("/schdule", new DashboardOptions()
			{
				Authorization = new[] { new HangfireAuthorizeFilter() },
				DashboardTitle = "sf task manager",
				DisplayStorageConnectionString = true
			});
			ConfigureHangfireServices();

			app.UseDeveloperExceptionPage();
			app.UseDatabaseErrorPage();
			DefaultFilesOptions options = new DefaultFilesOptions();
			options.DefaultFileNames.Add("index.html");    //将index.html改为需要默认起始页的文件名.
			app.UseDefaultFiles(options);
			//中间件方法
			app.UseStaticFiles();
			app.UseSession();

			//启用中间件服务生成Swagger作为JSON终结点
			app.UseSwagger();
			//启用中间件服务对swagger-ui，指定Swagger JSON终结点
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});
			// diabled Cors for chrome 80 not support
			// on dev mode , please just edit chrome setting : chrome://flags -> `SameSite by default cookies` : Disabled
			app.UseCors(MyAllowSpecificOrigins);
			app.UseCookiePolicy(new CookiePolicyOptions
			{
				MinimumSameSitePolicy = SameSiteMode.None,
			});
			app.UseAuthentication();

			//默认路由
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					//controller/action/param
					template: "{controller=Home}/{action=Cover}/{Id?}");
			});

			//seeder.Seed().Wait();
			//seeder.CreateUserRoles(services).Wait();
		}

		#endregion Logic
	}
}