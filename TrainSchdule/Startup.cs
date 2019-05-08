using System;
using System.IO;
using BLL.Interfaces;
using BLL.Services;
using BLL.Services.ApplyServices;
using DAL.Data;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Swagger;

namespace TrainSchdule
{
	public class Startup
    {
        #region Properties
        public IConfiguration Configuration { get; set; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        #endregion

        #region .ctors

        public Startup(IConfiguration configuration)
        {
			//注入Configuration服务
            Configuration = configuration;
        }

        #endregion

        #region Logic

        public void AddApplicationServices(IServiceCollection services)
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
			//单例
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IVerifyService,VerifyService>();
            services.AddSingleton<IFileProvider>(
	            new PhysicalFileProvider(Directory.GetCurrentDirectory()));
		}

		public void ConfigureServices(IServiceCollection services)
        {

			services.AddDbContext<ApplicationDbContext>(options=>{
				var connectionString = Configuration.GetConnectionString("DefaultConnection");
				options.UseLazyLoadingProxies()
					   .UseSqlServer(connectionString);
			});

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
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services) 
        {
            if (env.IsDevelopment())
            {
                
            }
            else if(env.IsProduction())
            {
               
            }
			app.UseDeveloperExceptionPage();
			app.UseDatabaseErrorPage();
			app.UseWelcomePage(new WelcomePageOptions() {
				Path="/welcome"
			});
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
                    template: "{controller=Home}/{action=Cover}/{id?}");
            });

            //seeder.Seed().Wait();
            //seeder.CreateUserRoles(services).Wait();

        }

        #endregion
    }
}
