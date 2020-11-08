using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using BLL.Crontab;
using BLL.Services;
using BLL.Services.Common;
using BLL.Services.File;
using Common.Text.Json.SystemTextJsonSamples;
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
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using TrainSchdule.Controllers.Log;
using TrainSchdule.Controllers.Statistics;
using TrainSchdule.Crontab;
using TrainSchdule.System;
using TsWebSocket.WebSockets;

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

		/// <summary>
		///
		/// </summary>
		public IWebHostEnvironment Env { get; }

		private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

		#endregion Properties

		#region .ctors

		/// <summary>
		///
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="env"></param>
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			//注入Configuration服务
			Configuration = configuration;
			Env = env;
		}

		#endregion .ctors

		#region Logic

		private void AddHangfireServices(IServiceCollection services)
		{
			var hangfire = new SqlServerStorageOptions
			{
				CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
				SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
				QueuePollInterval = TimeSpan.Zero,
				UseRecommendedIsolationLevel = true,
				UsePageLocksOnDequeue = true,
				DisableGlobalLocks = true
			};
			var connection = Configuration.GetConnectionString("HangfireConnection");
			// Add Hangfire services.
			services.AddHangfire(configuration => configuration
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseSqlServerStorage(connection, hangfire));

			// Add the processing server as IHostedService
			services.AddHangfireServer();
		}

		private void ConfigureHangfireServices()
		{
			// UTC time need +8
			BackgroundJob.Enqueue<UserActionServices>(ua => ua.Log(UserOperation.FromSystemReport, "#System#", "Start", true, ActionRank.Infomation));
			RecurringJob.AddOrUpdate<ApplyClearJob>((a) => a.Run("OnJob"), Cron.Daily(16, 5));
			RecurringJob.AddOrUpdate<VacationStatisticsController>(a => a.ReloadAllStatistics(new DateTime(DateTime.Today.Year, 1, 1), DateTime.Today.AddDays(1)), Cron.Daily(17, 30));
			RecurringJob.AddOrUpdate<UserInfoClearJob>((a) => a.Run(), Cron.Hourly);
			RecurringJob.AddOrUpdate<FileServices>((u) => u.RemoveTimeoutUploadStatus(), Cron.Hourly);
			BackgroundJob.Schedule<ApplyClearJob>((a) => a.Run("OnStart"), TimeSpan.FromMinutes(5));
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
			services.RegisterServices();

			services
				.AddMvc(option =>
			{
				option.Filters.Add<ActionStatusMessageExceptionFilter>();
				option.MaxValidationDepth = 1024; // https://stackoverflow.com/questions/63112368/asp-net-core-api-validationvisitor-exceeded-the-maximum-configured-validation
			})
				.AddNewtonsoftJson(opt =>
				{
					opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
				});
			//.AddJsonOptions(opt =>
			//	{
			//		opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
			//		opt.JsonSerializerOptions.Converters.Add(new DictionaryTKeyEnumTValueConverter());
			//		opt.JsonSerializerOptions.AllowTrailingCommas = true;
			//	}
			//);
			//.AddJsonOptions(opt => opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
			// TODO use yyyy-mm-ddThh:ii:ssZ (UTC)
			services.AddWebSocketManager();
		}

		private void AddSwaggerServices(IServiceCollection services)
		{
			//注册Swagger生成器，定义一个和多个Swagger 文档
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "TrainSchdule", Version = "v1" });
				// 为 Swagger JSON and UI设置xml文档注释路径
				var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
				var xmlPath = Path.Combine(basePath, "TrainSchdule.xml");
				c.IncludeXmlComments(xmlPath, true);
			});
		}

		private void AddAllowCorsServices(IServiceCollection services)
		{
			var allowOrgin = Configuration.GetSection("Cors")?.GetValue<string>("Orgins", "localhost");
			var allowOrgins = allowOrgin.Split(',').Select(ip => ip.StartsWith("http") ? ip : $"http://{ip}").ToArray();
			services.AddCors(options =>
			{
				options.AddPolicy(
					MyAllowSpecificOrigins,
				builder => builder
				.WithOrigins(allowOrgins)
				.AllowAnyMethod()
				.AllowCredentials()
				.AllowAnyHeader()
				);
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
					options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
				});
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();
			services.AddSession(s =>
			{
				s.IdleTimeout = TimeSpan.FromMinutes(60);
				s.Cookie.SameSite = SameSiteMode.None;
				s.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
			});
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
				options.OnAppendCookie = cookieContext =>
					CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
				options.OnDeleteCookie = cookieContext =>
					CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
			});
		}

		private void CheckSameSite(HttpContext httpContext, CookieOptions options)
		{
			if (options.SameSite == SameSiteMode.None)
			{
				var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
				if (MyUserAgentDetectionLib.DisallowsSameSiteNone(userAgent))
				{
					options.SameSite = SameSiteMode.Unspecified;
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="serviceProvider"></param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
		{
			app.UseHangfireServer();
			var code = Configuration["Configuration:Permission:schdule_authcode"];
			app.UseHangfireDashboard("/schdule", new DashboardOptions()
			{
				Authorization = new[] { new HangfireAuthorizeFilter(code) },
				DashboardTitle = "sf task manager",
				DisplayStorageConnectionString = false,
			});
			ConfigureHangfireServices();

			app.UseDeveloperExceptionPage();

			DefaultFilesOptions options = new DefaultFilesOptions();
			options.DefaultFileNames.Add("index.html");    //将index.html改为需要默认起始页的文件名.
			app.UseDefaultFiles(options);

			// 中间件方法 #1
			app.UseStaticFiles();
			app.UseSession();

			// 默认路由 #2
			app.UseRouting();

			// 跨域 #3
			// diabled Cors for chrome 80 not support
			// on dev mode , please just edit chrome setting : chrome://flags -> `SameSite by default cookies` : Disabled
			app.UseCors(MyAllowSpecificOrigins);
			app.UseCookiePolicy(new CookiePolicyOptions
			{
				OnAppendCookie = cookieContext =>
					  CheckSameSite(cookieContext.Context, cookieContext.CookieOptions),
				OnDeleteCookie = cookieContext =>
					  CheckSameSite(cookieContext.Context, cookieContext.CookieOptions),
				MinimumSameSitePolicy = SameSiteMode.None,
				Secure = CookieSecurePolicy.SameAsRequest
			});

			// 认证 #4
			app.UseAuthentication();
			app.UseAuthorization();

			// 路由 #5
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			// websocket
			var wsOptions = new WebSocketOptions()
			{
				KeepAliveInterval = TimeSpan.FromSeconds(60),
				ReceiveBufferSize = 4 * 1024
			};
			app.UseWebSockets(wsOptions);
			// 消息处理中心
			app.MapWebSocketManager("/nebula/message", serviceProvider.GetService<MessageNotifyHandler>());

			// 其他中间件 #n
			// 启用中间件服务生成Swagger作为JSON终结点
			app.UseSwagger(a =>
			{
				// a.PreSerializeFilters.Add((swdoc, http) => swdoc.);
			});
			// 启用中间件服务对swagger-ui，指定Swagger JSON终结点
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});
			//seeder.Seed().Wait();
			//seeder.CreateUserRoles(services).Wait();
		}

		#endregion Logic
	}
}