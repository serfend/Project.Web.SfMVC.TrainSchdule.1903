using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data
{
	public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContextFactory()
		{
		}

		/// <summary>
		/// reference http://test.tnblog.net/hb/article/details/3366 solve for migration EFCore2.x to EFCore3.x
		/// Exception:Unable to create an object of type 'Context'. For the different patterns supp ...
		/// Breaking Changes: https://docs.microsoft.com/zh-cn/ef/core/what-is-new/ef-core-3.0/breaking-changes#query-execution-is-logged-at-debug-level-reverted
		/// 保持配置文件和DbContext在同一个设计时配置，更加安全
		/// </summary>
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var builder = new ConfigurationBuilder();
			builder.AddJsonFile("appsettings.json", optional: true);
			var Configuration = builder.Build();
			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseLazyLoadingProxies()
				   .UseSqlServer(connectionString);
			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}
}