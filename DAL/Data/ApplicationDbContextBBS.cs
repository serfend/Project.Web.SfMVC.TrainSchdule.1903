using DAL.Entities.BBS;
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
	}
}
