using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.ApplyServices
{
	public class ApplyAuditStreamServices : IApplyAuditStreamServices
	{
		private readonly ApplicationDbContext context;

		public ApplyAuditStreamServices(ApplicationDbContext context)
		{
			this.context = context;
		}

		public void EditNode(string nodeName, Func<ApplyAuditStreamNodeAction, bool> callback)
		{
			if (callback == null) return;
			var node = context.ApplyAuditStreamNodeActions.Where(a => a.Name == nodeName).FirstOrDefault();
			if (callback.Invoke(node)) context.SaveChanges();
		}

		public void EditSolution(string solutionName, Func<ApplyAuditStream, bool> callback)
		{
			if (callback == null) return;
			var solution = context.ApplyAuditStreams.Where(s => s.Name == solutionName).FirstOrDefault();
			if (callback.Invoke(solution)) context.SaveChanges();
		}

		public void EditSolutionRule(string solutionRuleName, Func<ApplyAuditStreamSolutionRule, bool> callback)
		{
			if (callback == null) return;
			var rule = context.ApplyAuditStreamSolutionRules.Where(r => r.Name == solutionRuleName).FirstOrDefault();
			if (callback.Invoke(rule)) context.SaveChanges();
		}

		public ApplyAuditStreamSolutionRule GetAuditSolutionRule(User user)
		{
			if (user == null) return null;
			var cmp = user.CompanyInfo.Company.Code;
			// 寻找符合条件的方案，并按优先级排序后取第一个
			var auditRule = context.ApplyAuditStreamSolutionRules.Where(s => GetToAuditMembers(cmp, s).Contains(user.Id)).OrderByDescending(a => a.Priority).FirstOrDefault();
			return auditRule;
		}

		public ApplyAuditStreamNodeAction NewNode(MembersFilter filter, string name, string description = null)
		{
			var prev = context.ApplyAuditStreamNodeActions.Where(a => a.Name == name).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStreamNodeAction()
			{
				Description = description,
				Name = name,
				Create = DateTime.Now,

				Companies = filter?.Companies,
				CompanyRefer = filter?.CompanyRefer,
				Duties = filter?.Duties,
				DutyIsMajor = filter?.DutyIsMajor ?? DutiesIsMajor.BothCanGo,
				AuditMembers = filter?.AuditMembers,
				AuditMembersCount = filter?.AuditMembersCount ?? 0,
			};
			context.ApplyAuditStreamNodeActions.Add(result);
			context.SaveChanges();
			return result;
		}

		public ApplyAuditStream NewSolution(IEnumerable<ApplyAuditStreamNodeAction> Nodes, string name, string description = null)
		{
			var prev = context.ApplyAuditStreams.Where(a => a.Name == name).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStream()
			{
				Name = name,
				Description = description,
				Create = DateTime.Now,

				Nodes = Nodes
			};
			context.ApplyAuditStreams.Add(result);
			context.SaveChanges();
			return result;
		}

		public ApplyAuditStreamSolutionRule NewSolutionRule(ApplyAuditStream solution, MembersFilter filter, string name, string description = null, int priority = 0, bool enable = false)
		{
			var prev = context.ApplyAuditStreamSolutionRules.Where(r => r.Name == name).FirstOrDefault();
			if (prev != null) return prev;
			var result = new ApplyAuditStreamSolutionRule()
			{
				Name = name,
				Description = description,
				Priority = priority,
				Enable = enable,

				Companies = filter?.Companies,
				CompanyRefer = filter?.CompanyRefer,
				Duties = filter?.Duties,
				DutyIsMajor = filter?.DutyIsMajor ?? DutiesIsMajor.BothCanGo,
				AuditMembers = filter?.AuditMembers,
				AuditMembersCount = filter?.AuditMembersCount ?? 0,
				Solution = solution
			};
			context.ApplyAuditStreamSolutionRules.Add(result);
			context.SaveChanges();
			return result;
		}

		public IEnumerable<string> GetToAuditMembers(string company, MembersFilter filter)
		{
			if (filter.AuditMembers != null) return filter.AuditMembers;
			var result = context.AppUsers.AsQueryable();

			// 指定单位
			string target = null;
			if (filter.CompanyRefer != null)
			{
				var refer = filter.CompanyRefer.ToLower();
				target = refer == "parent" ? company.Substring(0, company.Length - 1) : refer == "self" ? company : null;
			}
			if (target == null)
			{
				if (filter.Companies != null) result = result.Where(u => filter.Companies.Contains(u.CompanyInfo.Company.Code));
			}
			else
				result = result.Where(u => u.CompanyInfo.Company.Code == target);

			// 指定职务
			if (filter.Duties != null) result = result.Where(u => filter.Duties.Contains(u.CompanyInfo.Duties.Code));
			switch (filter.DutyIsMajor)
			{
				case DutiesIsMajor.BothCanGo: break;
				case DutiesIsMajor.OnlyMajor:
					result = result.Where(u => u.CompanyInfo.Duties.IsMajorManager);
					break;

				case DutiesIsMajor.OnlyUnMajor:
					result = result.Where(u => !u.CompanyInfo.Duties.IsMajorManager);
					break;
			}
			return result.Select(u => u.Id);
		}
	}
}