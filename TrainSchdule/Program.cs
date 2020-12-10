using DAL.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using System.Security.Authentication;

namespace TrainSchdule
{
	/// <summary>
	///
	/// </summary>
	public class Program
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			var host = new HostBuilder()
		   .UseContentRoot(Directory.GetCurrentDirectory())
		   .ConfigureWebHostDefaults(webBuilder =>
		   {
			   //webBuilder.UseKestrel((context, serverOptions) =>
			   //{
				  // serverOptions.Configure(context.Configuration.GetSection("Kestrel"))
					 //  .Endpoint("HTTPS", listenOptions =>
					 //  {
						//   listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12;
					 //  });
				  // serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
			   //});
			   webBuilder
			   .UseIISIntegration()
			   .UseStartup<Startup>();
		   })
		   .ConfigureAppConfiguration(ConfigConfiguration)
		   .ConfigureLogging(ConfigLogging)
		   .Build();

			using (var scope = host.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				db.Database.Migrate();
			}

			host.Run();
		}

		/// <summary>
		/// 配置 配置文件
		/// </summary>
		/// <param name="hostingContext"></param>
		/// <param name="builder"></param>
		private static void ConfigConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder builder)
		{
			var env = hostingContext.HostingEnvironment;
			builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

			if (env.IsDevelopment() && !string.IsNullOrEmpty(env.ApplicationName))
			{
				var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
				if (appAssembly != null)
				{
					builder.AddUserSecrets(appAssembly, optional: true);
				}
			}
		}

		/// <summary>
		/// 配置日志
		/// </summary>
		/// <param name="hostingContext"></param>
		/// <param name="builder"></param>
		private static void ConfigLogging(HostBuilderContext hostingContext, ILoggingBuilder builder)
		{
			builder.ClearProviders();
			builder.SetMinimumLevel(LogLevel.Trace);
			builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
			builder.AddConsole();
			builder.AddDebug();
		}
	}
}