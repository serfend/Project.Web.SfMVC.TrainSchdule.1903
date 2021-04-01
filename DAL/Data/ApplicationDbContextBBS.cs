using DAL.Entities.BBS;
using DAL.Entities.Common.Message;
using DAL.Entities.UserInfo.UserAppMessage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
   public partial class ApplicationDbContext
    {
		public DbSet<DAL.Entities.BBS.PostContent> PostContents { get; set; }
		public IQueryable<DAL.Entities.BBS.PostContent> PostContentsDb => PostContents.ToExistDbSet();
		public DbSet<DAL.Entities.BBS.PostInteractStatus> PostInteracts { get; set; }
		public DbSet<AppMessage> BBSMessages { get; set; }
		public DbSet<AppUserRelate> AppUserRelates { get; set; }
		public DbSet<UserAppMessageInfo> UserAppMessageInfos { get; set; }
	}
}
