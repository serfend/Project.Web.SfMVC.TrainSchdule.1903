using DAL.Entities;
using DAL.Entities.ApplyInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Data
{
	public partial class ApplicationDbContext
	{
		public DbSet<RecallOrder> RecallOrders { get; set; }
		public DbSet<Apply> Applies { get; set; }
		public IQueryable<Apply> AppliesDb { get => Applies.Where(a => !a.IsRemoved); }
		public DbSet<ApplyAuditStream> ApplyAuditStreams { get; set; }
		public DbSet<ApplyAuditStreamSolutionRule> ApplyAuditStreamSolutionRules { get; set; }
		public DbSet<ApplyAuditStreamNodeAction> ApplyAuditStreamNodeActions { get; set; }
		public DbSet<ApplyAuditStep> ApplyAuditSteps { get; set; }
		public DbSet<ApplyResponse> ApplyResponses { get; set; }
		public DbSet<ApplyRequest> ApplyRequests { get; set; }
		public DbSet<ApplyBaseInfo> ApplyBaseInfos { get; set; }
	}
}