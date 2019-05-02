using System;
using System.IO;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Repositories;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.BLL.Services;

namespace TrainSchdule.WEB
{
    public class Startup
    {
        #region Properties
        public IConfiguration Configuration { get; }
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
            services.AddTransient<ApplicationDbContextSeeder>();

			//每个http请求对应一个实例
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<ILikesService, LikesService>();
            services.AddScoped<IPhotosService, PhotosService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITagsService, TagsService>();
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddScoped<IStudentService, StudentService>();
			services.AddScoped<ICompaniesService, CompaniesService > ();
			services.AddScoped<IApplyService,ApplyService>();
			services.AddScoped< IGoogleAuthService, GoogleAuthService>();
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
			services.AddCors(options =>
			{
				options.AddPolicy(MyAllowSpecificOrigins,
					builder =>
					{
						builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed((x) =>
						{
							return true;
						});
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services, ApplicationDbContextSeeder seeder) 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else if(env.IsProduction())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHttpsRedirection();
            }
           
			app.UseWelcomePage(new WelcomePageOptions() {
				Path="/welcome"
			});
			//中间件方法
            app.UseStaticFiles();
            app.UseSession();


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
