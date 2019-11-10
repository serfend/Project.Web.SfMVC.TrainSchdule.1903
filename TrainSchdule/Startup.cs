﻿using System;
using System.IO;
using BLL.Interfaces;
using BLL.Services;
using BLL.Services.ApplyServices;
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
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Swagger;
using TrainSchdule.Crontab;

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
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        #endregion

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

		#endregion

		#region Logic

		private void AddApplicationServices(IServiceCollection services)
        {
			//每次调用均对应一个实例
            services.AddTransient<IEmailSender, EmailSender>();

			//每个http请求对应一个实例
            services.AddScoped<IUsersService, UsersService>();
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddScoped<ICompaniesService, CompaniesService > ();
			services.AddScoped<IApplyService,ApplyService>();
			services.AddScoped< IGoogleAuthService, GoogleAuthService>();
			services.AddScoped< ICompanyManagerServices, CompanyManagerServices>();
			services.AddScoped<IVocationCheckServices, VocationCheckServices>();
			//单例
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IVerifyService,VerifyService>();
            services.AddSingleton<IFileProvider>(
	            new PhysicalFileProvider(Directory.GetCurrentDirectory()));
            
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
		private  void ConfigureHangfireServices()
		{
			RecurringJob.AddOrUpdate<ApplyClearJob>((a) => a.Run(),"*/10 * * * *");
			RecurringJob.AddOrUpdate<NewYearVocationUpdateJob>((u) => u.Run(), "* * * 1 *");
			RecurringJob.AddOrUpdate<WeeklyVocationStatstics>((u) => u.Run(), "* * * * 6");
			RecurringJob.AddOrUpdate<MonthlyVocationStatstics>((u) => u.Run(), "* * 1 * *");
			RecurringJob.AddOrUpdate<SeasonlyVocationStatistics>((u) => u.Run(), "* * 1 * *");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
        {

			services.AddDbContext<ApplicationDbContext>(options=>{
				var connectionString = Configuration.GetConnectionString("DefaultConnection");
				options.UseLazyLoadingProxies()
					   .UseSqlServer(connectionString);
			});
			services.AddTimedJob();
			AddHangfireServices(services);
			AddAllowCorsServices(services);
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
            
          
			services.AddMvc();

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
				c.IncludeXmlComments(xmlPath);
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
            else if(env.IsProduction())
            {
               
            }
            app.UseTimedJob();
			app.UseHangfireDashboard();
			ConfigureHangfireServices();

			app.UseDeveloperExceptionPage();
			app.UseDatabaseErrorPage();
			app.UseWelcomePage(new WelcomePageOptions() {
				Path="/welcome"
			});
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

        #endregion
    }
}
