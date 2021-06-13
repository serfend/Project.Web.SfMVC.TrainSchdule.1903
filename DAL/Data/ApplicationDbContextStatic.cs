using DAL.Entities.Common;
using DAL.Entities.FileEngine;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Data
{
	public partial class ApplicationDbContext
	{
		public DbSet<UserFile> UserFiles { get; set; }
		public DbSet<UserFileInfo> UserFileInfos { get; set; }
		public IQueryable<UserFileInfo> UserFileInfosDb => UserFileInfos.ToExistQueryable();
		public DbSet<FileUploadStatus> FileUploadStatuses { get; set; }
		public DbSet<UploadCache> UploadCaches { get; set; }
		public DbSet<ShortUrl> CommonShortUrl { get; set; }
		public IQueryable<ShortUrl> CommonShortUrlDb => CommonShortUrl.ToExistQueryable().Where(c => c.Expire > DateTime.Now);
		public DbSet<ShortUrlStatistics> CommonShortUrlStatistics { get; set; }
		public DbSet<CommonNavigate> CommonNavigates { get; set; }
	}
}